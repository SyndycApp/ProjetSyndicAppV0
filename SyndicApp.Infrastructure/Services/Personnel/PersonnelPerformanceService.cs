using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Services.Personnel
{
    public class PersonnelPerformanceService : IPersonnelPerformanceService
    {
        private readonly ApplicationDbContext _db;

        public PersonnelPerformanceService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<PerformanceDto> GetScoreAsync(Guid employeId)
        {
            var presences = await _db.Presences
                .Where(p => p.UserId == employeId)
                .ToListAsync();

            var missions = await _db.PlanningMissions
                .Where(m => m.EmployeId == employeId)
                .Include(m => m.Validation)
                .ToListAsync();

            double score =
                (missions.Count(m => m.Validation != null) * 2) +
                (presences.Count(p => p.HeureFin != null));

            return new PerformanceDto(employeId, score);
        }
    }

}
