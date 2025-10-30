using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Incidents;

namespace SyndicApp.Application.Interfaces.Incidents
{
    public interface IIncidentService
    {
        Task<IncidentDto?> CreateAsync(IncidentCreateDto dto);
        Task<IncidentDto?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<IncidentDto?>> GetAllAsync(int page = 1, int pageSize = 50);
        Task<IReadOnlyList<IncidentDto?>> GetByResidenceAsync(Guid residenceId, int page = 1, int pageSize = 20);
        Task<IncidentDto?> UpdateAsync(Guid id, IncidentUpdateDto dto);
        Task<IncidentDto?> ChangeStatusAsync(Guid id, IncidentChangeStatusDto dto);
        Task DeleteAsync(Guid id); // interdit si Cloture
    }
}
