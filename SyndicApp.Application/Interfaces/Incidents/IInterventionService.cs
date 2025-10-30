using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Interventions;

namespace SyndicApp.Application.Interfaces.Incidents
{
    public interface IInterventionService
    {
        Task<InterventionDto?> CreateAsync(InterventionCreateDto dto);
        Task<InterventionDto?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<InterventionDto?>> GetAllAsync(int page = 1, int pageSize = 50);
        Task<IReadOnlyList<InterventionDto?>> GetByResidenceAsync(Guid residenceId, int page = 1, int pageSize = 20);
        Task<InterventionDto?> ChangeStatusAsync(Guid id, InterventionChangeStatusDto dto);
        Task DeleteAsync(Guid id);
    }
}
