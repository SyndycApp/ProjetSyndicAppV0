using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Enums.Assemblees;

namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class VoteService : IVoteService
    {
        private readonly ApplicationDbContext _db;
        private readonly IQuorumService _quorumService;

        public VoteService(
            ApplicationDbContext db,
            IQuorumService quorumService)
        {
            _db = db;
            _quorumService = quorumService;
        }


        public async Task<ResultatVoteDto> CalculerResultatAsync(Guid resolutionId)
        {
            var votes = await _db.Votes
                .Where(v => v.ResolutionId == resolutionId)
                .ToListAsync();

            if (!votes.Any())
                throw new InvalidOperationException("Aucun vote enregistré pour cette résolution.");

            var totalPour = votes
                .Where(v => v.Choix == ChoixVote.Pour)
                .Sum(v => v.PoidsVote);

            var totalContre = votes
                .Where(v => v.Choix == ChoixVote.Contre)
                .Sum(v => v.PoidsVote);

            var totalAbstention = votes
                .Where(v => v.Choix == ChoixVote.Abstention)
                .Sum(v => v.PoidsVote);

            var totalExprime = totalPour + totalContre;

            var estAdoptee = totalPour > (totalExprime / 2);

            return new ResultatVoteDto(
                resolutionId,
                totalPour,
                totalContre,
                totalAbstention,
                totalExprime,
                estAdoptee
            );
        }

        public async Task VoteAsync(Guid userId, VoteDto dto)
        {
            // 🔎 Charger la résolution + AG
            var resolution = await _db.Resolutions
                .Include(r => r.AssembleeGenerale)
                .FirstOrDefaultAsync(r => r.Id == dto.ResolutionId);

            if (resolution == null)
                throw new InvalidOperationException("Résolution introuvable");

            // 🚫 Blocage si quorum non atteint
            if (!await _quorumService.QuorumAtteintAsync(resolution.AssembleeGeneraleId))
                throw new InvalidOperationException("Quorum non atteint");

            // 🚫 Interdire le vote au donneur de procuration
            var aDonneProcuration = await _db.Procurations.AnyAsync(p =>
                p.AssembleeGeneraleId == resolution.AssembleeGeneraleId &&
                p.DonneurId == userId);

            if (aDonneProcuration)
                throw new InvalidOperationException(
                    "Vous avez donné procuration, vous ne pouvez pas voter.");

            // 🚫 Empêcher double vote
            var exists = await _db.Votes.AnyAsync(v =>
                v.ResolutionId == dto.ResolutionId &&
                v.UserId == userId);

            if (exists)
                throw new InvalidOperationException("Vote déjà existant");

            // 🔢 Calcul du poids réel du vote

            // Poids du lot principal
            var poids = await _db.Lots
                .Where(l => l.Id == dto.LotId)
                .Select(l => l.Tantiemes)
                .FirstAsync();

            // Ajouter les tantièmes des procurations reçues
            var procurations = await _db.Procurations
                .Where(p =>
                    p.AssembleeGeneraleId == resolution.AssembleeGeneraleId &&
                    p.MandataireId == userId)
                .Join(_db.Lots,
                    p => p.LotId,
                    l => l.Id,
                    (p, l) => l.Tantiemes)
                .SumAsync();

            poids += procurations;

            // 🗳 Création du vote juridiquement pondéré
            var vote = new Vote
            {
                ResolutionId = dto.ResolutionId,
                UserId = userId,
                LotId = dto.LotId,
                Choix = dto.Choix,
                PoidsVote = poids,
                DateVote = DateTime.UtcNow
            };

            _db.Votes.Add(vote);
            await _db.SaveChangesAsync();
        }
    }
}
