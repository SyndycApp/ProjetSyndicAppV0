using SyndicApp.Application.DTOs.Personnel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Personnel
{
    public interface IPersonnelScoreHistoryService
    {
        Task GenerateMonthlyScoreAsync(Guid employeId, int annee, int mois);
        Task<IReadOnlyList<ScoreMensuelDto>> GetHistoryAsync(Guid employeId);
    }
}
