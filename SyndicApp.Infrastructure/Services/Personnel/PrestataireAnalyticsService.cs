using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;

namespace SyndicApp.Infrastructure.Services.Personnel
{
    public class PrestataireAnalyticsService : IPrestataireAnalyticsService
    {
        private readonly ApplicationDbContext _db;

        public PrestataireAnalyticsService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<PrestataireStatsDto> GetStatsAsync(
            Guid prestataireId,
            DateOnly from,
            DateOnly to)
        {
            var fromDt = from.ToDateTime(TimeOnly.MinValue);
            var toDt = to.ToDateTime(TimeOnly.MaxValue);

            // =========================
            // 🔎 Interventions du prestataire
            // =========================
            var interventions = await _db.Interventions
                .AsNoTracking()
                .Where(i =>
                    i.PrestataireId == prestataireId &&
                    i.DatePrevue != null &&
                    i.DateRealisation != null &&
                    i.DatePrevue >= fromDt &&
                    i.DatePrevue <= toDt)
                .Select(i => new
                {
                    i.DatePrevue,
                    i.DateRealisation,
                    i.CoutReel
                })
                .ToListAsync();

            if (!interventions.Any())
            {
                return new PrestataireStatsDto(
                    NbInterventions: 0,
                    DelaiMoyenJours: 0,
                    CoutTotal: 0);
            }

            var nbInterventions = interventions.Count;

            var delaiMoyen = interventions.Average(i =>
                (i.DateRealisation!.Value - i.DatePrevue!.Value).TotalDays);

            var coutTotal = interventions.Sum(i =>
                i.CoutReel ?? 0);

            return new PrestataireStatsDto(
                NbInterventions: nbInterventions,
                DelaiMoyenJours: Math.Round(delaiMoyen, 2),
                CoutTotal: coutTotal
            );
        }
    }
}
