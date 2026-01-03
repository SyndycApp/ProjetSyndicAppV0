using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Application.Interfaces.Common;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Enums.Assemblees;

namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class ClotureAssembleeService : IClotureAssembleeService
    {
        private readonly ApplicationDbContext _db;
        private readonly IDecisionService _decisionService;
        private readonly INotificationService _notificationService;

        public ClotureAssembleeService(
            ApplicationDbContext db,
            IDecisionService decisionService, INotificationService notificationService)
        {
            _db = db;
            _decisionService = decisionService;
            _notificationService = notificationService;
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
                    await _decisionService.CreerDecisionAsync(resolution.Id);
            }

            assemblee.Statut = StatutAssemblee.Cloturee;
            assemblee.DateCloture = DateTime.UtcNow;

            var userIds = await _db.PresenceAss
                    .Where(p => p.AssembleeGeneraleId == assembleeId)
                    .Select(p => p.UserId)
                    .Distinct()
                    .ToListAsync();

            foreach (var userId in userIds)
            {
                await _notificationService.NotifierAsync(
                    userId,
                    "Assemblée clôturée",
                    $"L’assemblée « {assemblee.Titre} » est maintenant clôturée.",
                    "CLOTURE_AG",
                    assembleeId,
                    "Assemblee"
                );
            }

            // 🔍 AUDIT
            _db.AuditLogs.Add(new AuditLog
            {
                UserId = syndicId,
                Action = "CLOTURE_AG",
                Cible = $"Assemblee:{assembleeId}",
                DateAction = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
        }
    }
}
