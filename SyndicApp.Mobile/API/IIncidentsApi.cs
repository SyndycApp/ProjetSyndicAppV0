using Refit;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Api
{
    public interface IIncidentsApi
    {
        [Post("/api/Incidents")]
        Task<IncidentDto> CreateAsync([Body] IncidentCreateRequest body);

        [Get("/api/Incidents")]
        Task<List<IncidentDto>> GetAllAsync();

        [Get("/api/Incidents/{id}")]
        Task<IncidentDto> GetByIdAsync(Guid id);

        [Put("/api/Incidents/{id}")]
        Task UpdateAsync(string id, [Body] IncidentUpdateRequest body);

        [Delete("/api/Incidents/{id}")]
        Task DeleteAsync(Guid id);

        [Get("/api/Incidents/by-residence/{residenceId}")]
        Task<List<IncidentDto>> GetByResidenceAsync(Guid residenceId);

        [Put("/api/Incidents/{id}/status")]
        Task UpdateStatusAsync(string id, [Body] IncidentStatusUpdateRequest body);

    }
}
