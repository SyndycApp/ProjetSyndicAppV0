using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;

namespace SyndicApp.Infrastructure.Services.Personnel
{
    public class PersonnelAnalyticsService : IPersonnelAnalyticsService
    {
        private readonly ApplicationDbContext _db;

        public PersonnelAnalyticsService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<PersonnelStatsDto> GetStatsAsync(Guid employeId)
        {
            var presences = await _db.Presences
                .Where(p => p.UserId == employeId)
                .ToListAsync();

            var total = presences.Count;
            var completes = presences.Count(p => p.HeureFin != null);

            return new PersonnelStatsDto
            {
                TauxPresence = total == 0 ? 0 : completes * 100 / total,
                HeuresTravaillees = presences.Sum(p =>
                    p.HeureFin.HasValue
                        ? (p.HeureFin.Value - p.HeureDebut!.Value).TotalHours
                        : 0)
            };
        }
    }

}
