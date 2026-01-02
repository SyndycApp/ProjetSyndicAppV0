using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;

namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class ProcurationService : IProcurationService
    {
        private readonly ApplicationDbContext _db;
        private const int MAX_PROCURATIONS = 3;

        public ProcurationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<ProcurationViewDto>> GetProcurationsAsync(Guid assembleeId)
        {
            return await _db.Procurations
                .Where(p => p.AssembleeGeneraleId == assembleeId)
                .Join(
                    _db.Users,
                    p => p.DonneurId,
                    d => d.Id,
                    (p, donneur) => new { p, donneur }
                )
                .Join(
                    _db.Users,
                    x => x.p.MandataireId,
                    m => m.Id,
                    (x, mandataire) => new { x.p, x.donneur, mandataire }
                )
                .Join(
                    _db.Lots,
                    x => x.p.LotId,
                    l => l.Id,
                    (x, lot) => new ProcurationViewDto(
                        x.donneur.FullName,
                        x.mandataire.FullName,
                        lot.Tantiemes
                    )
                )
                .ToListAsync();
        }

        public async Task DonnerProcurationAsync(Guid userId, CreateProcurationDto dto)
        {
            var count = await _db.Procurations.CountAsync(p =>
                p.AssembleeGeneraleId == dto.AssembleeId &&
                p.MandataireId == dto.MandataireId);

            if (count >= MAX_PROCURATIONS)
                throw new InvalidOperationException("Limite légale atteinte");

            _db.Procurations.Add(new Procuration
            {
                AssembleeGeneraleId = dto.AssembleeId,
                DonneurId = userId,
                MandataireId = dto.MandataireId,
                LotId = dto.LotId,
                DateCreation = DateTime.UtcNow
            });

            // 🔍 AUDIT
            _db.AuditLogs.Add(new AuditLog
            {
                UserId = userId,
                Action = "PROCURATION",
                Cible = $"Assemblee:{dto.AssembleeId}",
                DateAction = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
        }
    }
}
