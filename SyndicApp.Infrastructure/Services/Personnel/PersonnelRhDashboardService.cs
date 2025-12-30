using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Services.Personnel
{
    public class PersonnelRhDashboardService : IPersonnelRhDashboardService
    {
        private readonly ApplicationDbContext _db;

        public PersonnelRhDashboardService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<RhDashboardDetailsDto> GetDetailsAsync(
            Guid employeId,
            DateOnly dateFrom,
            DateOnly to)
        {
            var dateFromDt = dateFrom.ToDateTime(TimeOnly.MinValue);
            var dateToDt = to.ToDateTime(TimeOnly.MaxValue);

            // =========================
            // ⏱ Heures prévues (planning)
            // =========================
            var planning = await _db.PlanningMissions
                .Where(m =>
                    m.EmployeId == employeId &&
                    m.Date >= dateFrom &&
                    m.Date <= to)
                .Select(m => new
                {
                    m.Date,
                    m.HeureDebut,
                    m.HeureFin
                })
                .ToListAsync();

            var minutesPrevues = planning.Sum(m =>
            {
                var debut = m.Date.ToDateTime(TimeOnly.MinValue).Add(m.HeureDebut);
                var fin = m.Date.ToDateTime(TimeOnly.MinValue).Add(m.HeureFin);
                return (int)(fin - debut).TotalMinutes;
            });

            var heuresPrevues = minutesPrevues / 60.0;

            // =========================
            // 🔗 UserId lié à l’employé
            // =========================
            var userId = await _db.Employes
                .Where(e => e.Id == employeId)
                .Select(e => e.UserId)
                .FirstAsync();

            // =========================
            // ⏱ Heures réelles (présences)
            // =========================
            var presences = await _db.Presences
                .Where(p =>
                    p.UserId == userId &&
                    p.HeureDebut != null &&
                    p.HeureFin != null &&
                    p.HeureDebut >= dateFromDt &&
                    p.HeureDebut <= dateToDt)
                .Select(p => new
                {
                    p.HeureDebut,
                    p.HeureFin
                })
                .ToListAsync();

            var minutesReelles = presences.Sum(p =>
                (int)(p.HeureFin!.Value - p.HeureDebut!.Value).TotalMinutes);

            var heuresReelles = minutesReelles / 60.0;

            // =========================
            // ⏰ Retards
            // =========================
            var retardsMinutes = await (
    from p in _db.Presences
    join m in _db.PlanningMissions
        on p.PlanningMissionId equals m.Id
    where p.UserId == userId
       && p.HeureDebut != null
       && m.Date >= dateFrom
       && m.Date <= to
    select new
    {
        Prevue = m.Date.ToDateTime(TimeOnly.MinValue)
                       .Add(m.HeureDebut),
        Reelle = p.HeureDebut!.Value
    }
).ToListAsync();

            var retards = retardsMinutes
                .Select(r => (int)(r.Reelle - r.Prevue).TotalMinutes)
                .Where(min => min > 0)
                .ToList();

            // =========================
            // ❌ Absences non justifiées
            // =========================
            var absencesNonJustifiees = await _db.PlanningMissions
                .Where(m =>
                    m.EmployeId == employeId &&
                    m.Date >= dateFrom &&
                    m.Date <= to)
                .CountAsync(m =>
                    !_db.Presences.Any(p => p.PlanningMissionId == m.Id)
                    && !_db.AbsenceJustifications.Any(a =>
                        a.UserId == userId &&
                        a.Date == m.Date &&
                        a.Validee));

            return new RhDashboardDetailsDto(
                new HeuresPrevuesVsReellesDto(
                    heuresPrevues,
                    heuresReelles),
                new RetardStatsDto(
                    retards.Count,
                    retards.Sum() / 60.0),
                absencesNonJustifiees
            );
        }
    }
}
