using SyndicApp.Application.DTOs.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Personnel
{
    public interface IPersonnelPerformanceService
    {
        Task<PerformanceDto> GetScoreAsync(Guid employeId);
    }
}
