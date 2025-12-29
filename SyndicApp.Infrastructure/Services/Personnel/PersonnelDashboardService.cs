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
    public class PersonnelDashboardService : IPersonnelDashboardService
    {
        private readonly ApplicationDbContext _db;

        public PersonnelDashboardService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<DashboardPersonnelDto> GetAsync(Guid residenceId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            return new DashboardPersonnelDto(
                MissionsNonValidees: await _db.PlanningMissions.CountAsync(m =>
                    m.ResidenceId == residenceId && m.Validation == null),

                AbsencesAujourdhui: await _db.AbsenceJustifications.CountAsync(a =>
                    a.Date == today && !a.Validee),

                PresencesEnCours: await _db.Presences.CountAsync(p =>
                    p.HeureFin == null)
            );
        }
    }

}
