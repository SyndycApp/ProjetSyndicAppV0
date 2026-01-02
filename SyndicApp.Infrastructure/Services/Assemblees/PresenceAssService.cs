using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Application.DTOs.Assemblees;

namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class PresenceAssService : IPresenceAssService
    {
        private readonly ApplicationDbContext _db;

        public PresenceAssService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task EnregistrerPresenceAsync(Guid userId, PresenceAssDto dto)
        {
            var exists = await _db.PresenceAss.AnyAsync(p =>
                p.AssembleeGeneraleId == dto.AssembleeId &&
                p.UserId == userId);

            if (exists)
                throw new InvalidOperationException("Présence déjà enregistrée");

            _db.PresenceAss.Add(new PresenceAss
            {
                AssembleeGeneraleId = dto.AssembleeId,
                UserId = userId,
                LotId = dto.LotId,
                Type = dto.Type,
                Tantiemes = 1,
                DatePresence = DateTime.UtcNow
            });

           
            _db.AuditLogs.Add(new AuditLog
            {
                UserId = userId,
                Action = "PRESENCE",
                Cible = $"Assemblee:{dto.AssembleeId}",
                DateAction = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
        }
    }
}
