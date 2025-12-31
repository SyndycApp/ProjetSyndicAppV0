using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;

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

            // 🚫 Empêcher double vote
            var exists = await _db.Votes.AnyAsync(v =>
                v.ResolutionId == dto.ResolutionId &&
                v.UserId == userId);

            if (exists)
                throw new InvalidOperationException("Vote déjà existant");

            // 🗳 Création du vote
            var vote = new Vote
            {
                ResolutionId = dto.ResolutionId,
                UserId = userId,
                LotId = dto.LotId,
                Choix = dto.Choix,
                PoidsVote = 1, // TODO : calculer depuis tantièmes du lot
                DateVote = DateTime.UtcNow
            };

            _db.Votes.Add(vote);
            await _db.SaveChangesAsync();
        }
    }
}
