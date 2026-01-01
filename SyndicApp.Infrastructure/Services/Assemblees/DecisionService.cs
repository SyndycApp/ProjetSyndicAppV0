using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;

namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class DecisionService : IDecisionService
    {
        private readonly ApplicationDbContext _db;
        private readonly IVoteService _service;

        public DecisionService(
            ApplicationDbContext db,
            IVoteService service)
        {
            _db = db;
            _service = service;
        }

        public async Task<DecisionDto> CreerDecisionAsync(Guid resolutionId)
        {
            var exists = await _db.Decisions
                .AnyAsync(d => d.ResolutionId == resolutionId);

            if (exists)
                throw new InvalidOperationException("Décision déjà créée pour cette résolution.");

            var resolution = await _db.Resolutions
                .Include(r => r.AssembleeGenerale)
                .FirstOrDefaultAsync(r => r.Id == resolutionId);

            if (resolution == null)
                throw new InvalidOperationException("Résolution introuvable.");

            var resultat = await _service.CalculerResultatAsync(resolutionId);

            var decision = new Decision
            {
                AssembleeGeneraleId = resolution.AssembleeGeneraleId,
                ResolutionId = resolutionId,
                TotalPour = resultat.TotalPour,
                TotalContre = resultat.TotalContre,
                TotalAbstention = resultat.TotalAbstention,
                TotalExprime = resultat.TotalExprime,
                EstAdoptee = resultat.EstAdoptee,
                DateDecision = DateTime.UtcNow
            };

            _db.Decisions.Add(decision);
            await _db.SaveChangesAsync();

            return new DecisionDto(
                resolutionId,
                resolution.AssembleeGeneraleId,
                resultat.TotalPour,
                resultat.TotalContre,
                resultat.TotalAbstention,
                resultat.TotalExprime,
                resultat.EstAdoptee,
                decision.DateDecision
            );
        }

        public async Task<List<DecisionDto>> GetDecisionsByAssembleeAsync(Guid assembleeId)
        {
            return await _db.Decisions
                .Where(d => d.AssembleeGeneraleId == assembleeId)
                .Select(d => new DecisionDto(
                    d.ResolutionId,
                    d.AssembleeGeneraleId,
                    d.TotalPour,
                    d.TotalContre,
                    d.TotalAbstention,
                    d.TotalExprime,
                    d.EstAdoptee,
                    d.DateDecision
                ))
                .ToListAsync();
        }
    }
}
