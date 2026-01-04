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
            var resolution = await _db.Resolutions
                .Include(r => r.AssembleeGenerale)
                .FirstOrDefaultAsync(r => r.Id == resolutionId);

            if (resolution == null)
                throw new InvalidOperationException("Résolution introuvable.");

            if (_accessPolicy.EstVerrouillee(resolution.AssembleeGenerale))
                throw new InvalidOperationException(
                    "Résultat figé : assemblée clôturée."
                );


            var votes = await _db.Votes
                .Where(v => v.ResolutionId == resolutionId)
                .ToListAsync();

            if (!votes.Any())
                throw new InvalidOperationException("Aucun vote enregistré pour cette résolution.");

            var totalPour = votes.Where(v => v.Choix == ChoixVote.Pour).Sum(v => v.PoidsVote);
            var totalContre = votes.Where(v => v.Choix == ChoixVote.Contre).Sum(v => v.PoidsVote);
            var totalAbstention = votes.Where(v => v.Choix == ChoixVote.Abstention).Sum(v => v.PoidsVote);
            var totalExprime = totalPour + totalContre;

            var totalTantiemes = await _db.Lots
                .Where(l => l.ResidenceId == resolution.AssembleeGenerale.ResidenceId)
                .SumAsync(l => l.Tantiemes);

            var adoptee = EstResolutionAdoptee(
                resolution,
                totalPour,
                totalContre,
                totalExprime,
                totalTantiemes
            );

            return new ResultatVoteDto(
                resolutionId,
                totalPour,
                totalContre,
                totalAbstention,
                totalExprime,
                adoptee
            );
        }


        public async Task VoteAsync(Guid userId, VoteDto dto)
        {
            var now = DateTime.UtcNow;
            var resolution = await _db.Resolutions
                .Include(r => r.AssembleeGenerale)
                .FirstOrDefaultAsync(r => r.Id == dto.ResolutionId);

            if (resolution == null)
                throw new InvalidOperationException("Résolution introuvable");

            if (_accessPolicy.EstVerrouillee(resolution.AssembleeGenerale))
                throw new InvalidOperationException("Assemblée clôturée : vote interdit.");


            if (!_accessPolicy.PeutVoter(resolution.AssembleeGenerale))
                throw new InvalidOperationException("Le vote est fermé pour cette assemblée.");

            if (!_accessPolicy.EstDansPlageHoraireVote(resolution.AssembleeGenerale, now))
                throw new InvalidOperationException("Le vote n’est autorisé que pendant la période officielle.");


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
                    v.Choix,
                    v.PoidsVote,
                    v.DateVote
                ))
                .ToListAsync();
        }


        private bool EstResolutionAdoptee(
    Resolution resolution,
    decimal totalPour,
    decimal totalContre,
    decimal totalExprime,
    decimal totalTantiemes)
        {
            return resolution.TypeMajorite switch
            {
                TypeMajorite.Simple =>
                    totalPour > (totalExprime / 2),

                TypeMajorite.Absolue =>
                    totalPour > (totalTantiemes / 2),

                TypeMajorite.Qualifiee =>
                    totalPour >= (totalTantiemes * 0.66m),

                TypeMajorite.Unanimite =>
                    totalContre == 0 && totalExprime == totalPour,

                TypeMajorite.Personnalisee =>
                    resolution.SeuilMajorite.HasValue &&
                    totalPour >= (totalTantiemes * resolution.SeuilMajorite.Value),

                _ => false
            };
        }

    }
}
