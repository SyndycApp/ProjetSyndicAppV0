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
            var exists = await _db.PresenceAss
                .AnyAsync(p =>
                    p.AssembleeGeneraleId == dto.AssembleeId &&
                    p.UserId == userId);

            if (exists)
                throw new InvalidOperationException("Présence déjà enregistrée");

            var presence = new PresenceAss
            {
                AssembleeGeneraleId = dto.AssembleeId,
                UserId = userId,
                LotId = dto.LotId,
                Type = dto.Type,
                Tantiemes = 1, // TODO : depuis le lot
                DatePresence = DateTime.UtcNow
            };

            _db.PresenceAss.Add(presence);
            await _db.SaveChangesAsync();
        }
    }

}
