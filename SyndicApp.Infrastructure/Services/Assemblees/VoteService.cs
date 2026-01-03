using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Application.Interfaces.Common;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Enums.Assemblees;

namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class VoteService : IVoteService
    {
        private readonly ApplicationDbContext _db;
        private readonly IQuorumService _quorumService;
        private readonly IAssembleeAccessPolicy _accessPolicy;
        private readonly INotificationService _notificationService;

        public VoteService(
            ApplicationDbContext db,
            IQuorumService quorumService,
            IAssembleeAccessPolicy accessPolicy, INotificationService notificationService)
        {
            _db = db;
            _quorumService = quorumService;
            _accessPolicy = accessPolicy;
            _notificationService = notificationService;
        }

        public async Task<ResultatVoteDto> CalculerResultatAsync(Guid resolutionId)
        {
            var votes = await _db.Votes
                .Where(v => v.ResolutionId == resolutionId)
                .ToListAsync();

            if (!votes.Any())
                throw new InvalidOperationException("Aucun vote enregistré pour cette résolution.");

            var totalPour = votes.Where(v => v.Choix == ChoixVote.Pour).Sum(v => v.PoidsVote);
            var totalContre = votes.Where(v => v.Choix == ChoixVote.Contre).Sum(v => v.PoidsVote);
            var totalAbstention = votes.Where(v => v.Choix == ChoixVote.Abstention).Sum(v => v.PoidsVote);
            var totalExprime = totalPour + totalContre;

            return new ResultatVoteDto(
                resolutionId,
                totalPour,
                totalContre,
                totalAbstention,
                totalExprime,
                totalPour > (totalExprime / 2)
            );
        }

        public async Task VoteAsync(Guid userId, VoteDto dto)
        {

            var resolution = await _db.Resolutions
                .Include(r => r.AssembleeGenerale)
                .FirstOrDefaultAsync(r => r.Id == dto.ResolutionId);

            if (!_accessPolicy.PeutVoter(resolution.AssembleeGenerale))
                throw new InvalidOperationException("Le vote est fermé pour cette assemblée.");

            var now = DateTime.UtcNow;

            if (!_accessPolicy.EstDansPlageHoraireVote(resolution.AssembleeGenerale, now))
                throw new InvalidOperationException("Le vote n’est autorisé que pendant la période officielle de l’assemblée.");

            if (resolution == null)
                throw new InvalidOperationException("Résolution introuvable");

            if (!await _quorumService.QuorumAtteintAsync(resolution.AssembleeGeneraleId))
                throw new InvalidOperationException("Quorum non atteint");

            var aDonneProcuration = await _db.Procurations.AnyAsync(p =>
                p.AssembleeGeneraleId == resolution.AssembleeGeneraleId &&
                p.DonneurId == userId);

            if (aDonneProcuration)
                throw new InvalidOperationException("Vous avez donné procuration, vous ne pouvez pas voter.");

            var poids = await _db.Lots
                .Where(l => l.Id == dto.LotId)
                .Select(l => l.Tantiemes)
                .FirstAsync();

            var procurations = await _db.Procurations
                .Where(p =>
                    p.AssembleeGeneraleId == resolution.AssembleeGeneraleId &&
                    p.MandataireId == userId)
                .Join(_db.Lots, p => p.LotId, l => l.Id, (p, l) => l.Tantiemes)
                .SumAsync();

            poids += procurations;

            var vote = await _db.Votes.FirstOrDefaultAsync(v =>
                v.ResolutionId == dto.ResolutionId &&
                v.UserId == userId);

            if (vote != null)
            {
                vote.Choix = dto.Choix;
                vote.DateVote = DateTime.UtcNow;
                vote.PoidsVote = poids;
            }
            else
            {
                _db.Votes.Add(new Vote
                {
                    ResolutionId = dto.ResolutionId,
                    UserId = userId,
                    LotId = dto.LotId,
                    Choix = dto.Choix,
                    PoidsVote = poids,
                    DateVote = DateTime.UtcNow
                });
            }

            // 🔍 AUDIT
            _db.AuditLogs.Add(new AuditLog
            {
                UserId = userId,
                Action = "VOTE",
                Cible = $"Resolution:{dto.ResolutionId}",
                DateAction = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
            await _notificationService.NotifierAsync(
                        userId: userId,
                        titre: "Vote enregistré",
                        message: "Votre vote a été enregistré avec succès.",
                        type: "VOTE",
                        cibleId: dto.ResolutionId,
                        cibleType: "Resolution");

        }

        public async Task<List<VotePersonnelDto>> GetMesVotesAsync(Guid assembleeId, Guid userId)
        {
            _db.AuditLogs.Add(new AuditLog
            {
                UserId = userId,
                Action = "CONSULTATION_VOTES",
                Cible = $"Assemblee:{assembleeId}",
                DateAction = DateTime.UtcNow
            });

            return await _db.Votes
                .Where(v =>
                    v.UserId == userId &&
                    v.Resolution.AssembleeGeneraleId == assembleeId)
                .Include(v => v.Resolution)
                .OrderBy(v => v.Resolution.Numero)
                .Select(v => new VotePersonnelDto(
                    v.ResolutionId,
                    v.Resolution.Numero,
                    v.Resolution.Titre,
                    v.Choix.ToString(),
                    v.PoidsVote,
                    v.DateVote
                ))
                .ToListAsync();
        }

    }
}
