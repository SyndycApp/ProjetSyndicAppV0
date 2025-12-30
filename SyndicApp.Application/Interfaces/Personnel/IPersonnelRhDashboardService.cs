using System;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Personnel;

namespace SyndicApp.Application.Interfaces.Personnel
{
    public interface IPersonnelRhDashboardService
    {
        Task<RhDashboardDetailsDto> GetDetailsAsync(
            Guid employeId,
            DateOnly dateFrom,
            DateOnly to);

        Task<RhKpiDto> GetKpisAsync(
        Guid employeId,
        DateOnly dateFrom,
        DateOnly to);
    }
}
