using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.Domain.Entities.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Services.Personnel
{
    public class PersonnelScoreHistoryService : IPersonnelScoreHistoryService
    {
        private readonly ApplicationDbContext _db;
        private readonly IPersonnelPerformanceService _performanceService;

        public PersonnelScoreHistoryService(
            ApplicationDbContext db,
            IPersonnelPerformanceService performanceService)
        {
            _db = db;
            _performanceService = performanceService;
        }

        public async Task GenerateMonthlyScoreAsync(
            Guid employeId,
            int annee,
            int mois)
        {
            var performance = await _performanceService.GetScoreAsync(employeId);
            var scoreBrut = performance.ScoreFiabilite;

            var scoreNormalise =
                (int)Math.Min(100, Math.Round(scoreBrut * 10));

            var exists = await _db.PersonnelScoreHistoriques
                .AnyAsync(s =>
                    s.EmployeId == employeId &&
                    s.Annee == annee &&
                    s.Mois == mois);

            if (exists)
                return;

            var historique = new PersonnelScoreHistorique
            {
                Id = Guid.NewGuid(),
                EmployeId = employeId,
                Annee = annee,
                Mois = mois,
                ScoreBrut = scoreBrut,
                ScoreNormalise = scoreNormalise
            };

            _db.PersonnelScoreHistoriques.Add(historique);
            await _db.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<ScoreMensuelDto>> GetHistoryAsync(
            Guid employeId)
        {
            return await _db.PersonnelScoreHistoriques
                .Where(s => s.EmployeId == employeId)
                .OrderBy(s => s.Annee)
                .ThenBy(s => s.Mois)
                .Select(s => new ScoreMensuelDto(
                    s.Annee,
                    s.Mois,
                    s.ScoreNormalise))
                .ToListAsync();
        }
    }
}
