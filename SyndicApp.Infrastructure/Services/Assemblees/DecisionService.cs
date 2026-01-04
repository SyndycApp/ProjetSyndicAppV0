using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Enums.Assemblees;

namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class DecisionService : IDecisionService
    {
        private readonly ApplicationDbContext _db;

        public DecisionService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<DecisionDto> CreerDecisionAsync(Guid resolutionId)
        {
            var exists = await _db.Decisions
                .AnyAsync(d => d.ResolutionId == resolutionId);

            if (exists)
                throw new InvalidOperationException(
                    "Décision déjà créée pour cette résolution."
                );

            var resolution = await _db.Resolutions
                .Include(r => r.AssembleeGenerale)
                .FirstOrDefaultAsync(r => r.Id == resolutionId);

            if (resolution == null)
                throw new InvalidOperationException("Résolution introuvable.");

            if (resolution.AssembleeGenerale.Statut != StatutAssemblee.Cloturee)
                throw new InvalidOperationException(
                    "La décision ne peut être créée qu’après la clôture de l’assemblée."
                );

            // 🔒 CALCUL FIGÉ
            var votes = await _db.Votes
                .Where(v => v.ResolutionId == resolutionId)
                .ToListAsync();

            if (!votes.Any())
                throw new InvalidOperationException(
                    "Aucun vote enregistré pour cette résolution."
                );

            var totalPour = votes.Where(v => v.Choix == ChoixVote.Pour).Sum(v => v.PoidsVote);
            var totalContre = votes.Where(v => v.Choix == ChoixVote.Contre).Sum(v => v.PoidsVote);
            var totalAbstention = votes.Where(v => v.Choix == ChoixVote.Abstention).Sum(v => v.PoidsVote);
            var totalExprime = totalPour + totalContre;

            var estAdoptee = totalPour > (totalExprime / 2);

            var decision = new Decision
            {
                AssembleeGeneraleId = resolution.AssembleeGeneraleId,
                ResolutionId = resolutionId,
                Titre = resolution.Titre,
                Description = resolution.Description,
                TotalPour = totalPour,
                TotalContre = totalContre,
                TotalAbstention = totalAbstention,
                TotalExprime = totalExprime,
                EstAdoptee = estAdoptee,
                DateDecision = DateTime.UtcNow
            };

            _db.Decisions.Add(decision);
            await _db.SaveChangesAsync();

            return new DecisionDto(
                resolutionId,
                resolution.AssembleeGeneraleId,
                resolution.Titre,
                resolution.Description,
                totalPour,
                totalContre,
                totalAbstention,
                totalExprime,
                estAdoptee,
                decision.DateDecision
            );
        }

        public async Task<List<DecisionDto>> GetDecisionsByAssembleeAsync(Guid assembleeId)
        {
            var ag = await _db.AssembleesGenerales
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == assembleeId);

            if (ag == null)
                throw new InvalidOperationException("Assemblée introuvable.");

            if (ag.Statut != StatutAssemblee.Cloturee)
                throw new InvalidOperationException(
                    "Les décisions sont consultables uniquement après la clôture."
                );

            return await _db.Decisions
                .Where(d => d.AssembleeGeneraleId == assembleeId)
                .OrderBy(d => d.DateDecision)
                .Select(d => new DecisionDto(
                    d.ResolutionId,
                    d.AssembleeGeneraleId,
                    d.Titre,
                    d.Description,
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
