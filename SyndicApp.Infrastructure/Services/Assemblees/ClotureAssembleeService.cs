using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Enums.Assemblees;

namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class ClotureAssembleeService : IClotureAssembleeService
    {
        private readonly ApplicationDbContext _db;
        private readonly IDecisionService _decisionService;

        public ClotureAssembleeService(
            ApplicationDbContext db,
            IDecisionService decisionService)
        {
            _db = db;
            _decisionService = decisionService;
        }

        public async Task CloturerAsync(Guid assembleeId, Guid syndicId)
        {
            var assemblee = await _db.AssembleesGenerales
                .Include(a => a.Resolutions)
                .FirstOrDefaultAsync(a => a.Id == assembleeId);

            if (assemblee == null)
                throw new InvalidOperationException("Assemblée introuvable.");

            if (assemblee.Statut != StatutAssemblee.Ouverte)
                throw new InvalidOperationException("L’assemblée n’est pas ouverte.");

            foreach (var resolution in assemblee.Resolutions)
            {
                var decisionExiste = await _db.Decisions
                    .AnyAsync(d => d.ResolutionId == resolution.Id);

                if (!decisionExiste)
                {
                    await _decisionService.CreerDecisionAsync(resolution.Id);
                }
            }

            assemblee.Statut = StatutAssemblee.Cloturee;
            assemblee.DateCloture = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }
    }
}
