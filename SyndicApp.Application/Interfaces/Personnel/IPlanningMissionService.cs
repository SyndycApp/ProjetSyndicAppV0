using SyndicApp.Application.DTOs.Personnel;

namespace SyndicApp.Application.Interfaces.Personnel
{
    public interface IPlanningMissionService
    {
        Task<Guid> CreateAsync(CreatePlanningMissionDto dto);
        Task UpdateAsync(Guid id, UpdatePlanningMissionDto dto);
        Task DeleteAsync(Guid id);

        Task<IReadOnlyList<PlanningMissionDto>> GetByEmployeAsync(Guid employeId);
        Task<IReadOnlyList<PlanningMissionDto>> GetByResidenceAsync(Guid residenceId);
    }


}
