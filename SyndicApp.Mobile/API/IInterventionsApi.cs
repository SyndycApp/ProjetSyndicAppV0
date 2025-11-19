// SyndicApp.Mobile/Api/IInterventionsApi.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Api
{
    public interface IInterventionsApi
    {
        [Get("/api/Interventions")]
        Task<List<InterventionDto>> GetAllAsync(int page = 1, int pageSize = 50);

        [Get("/api/Interventions/{id}")]
        Task<InterventionDto> GetByIdAsync(Guid id);

        [Get("/api/Interventions/by-residence/{residenceId}")]
        Task<List<InterventionDto>> GetByResidenceAsync(Guid residenceId, int page = 1, int pageSize = 20);

        [Post("/api/Interventions")]
        Task<InterventionDto> CreateAsync([Body] InterventionCreateRequest dto);

        [Put("/api/Interventions/{id}/status")]
        Task<InterventionDto> ChangeStatusAsync(Guid id, [Body] InterventionChangeStatusRequest dto);

        [Delete("/api/Interventions/{id}")]
        Task DeleteAsync(Guid id);
    }
}
