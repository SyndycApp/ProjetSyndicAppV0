using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Api
{
    public interface IDevisTravauxApi
    {
        // GET /api/DevisTravaux?page=&pageSize=
        [Get("/api/DevisTravaux")]
        Task<List<DevisTravauxDto>> GetAllAsync(int page = 1, int pageSize = 50);

        // GET /api/DevisTravaux/{id}
        [Get("/api/DevisTravaux/{id}")]
        Task<DevisTravauxDto> GetByIdAsync(Guid id);

        // POST /api/DevisTravaux
        [Post("/api/DevisTravaux")]
        Task<DevisTravauxDto> CreateAsync([Body] DevisTravauxCreateRequest request);

        // DELETE /api/DevisTravaux/{id}
        [Delete("/api/DevisTravaux/{id}")]
        Task DeleteAsync(Guid id);

        // GET /api/DevisTravaux/by-residence/{residenceId}
        [Get("/api/DevisTravaux/by-residence/{residenceId}")]
        Task<List<DevisTravauxDto>> GetByResidenceAsync(Guid residenceId, int page = 1, int pageSize = 20);

        // PUT /api/DevisTravaux/{id}/decision
        [Put("/api/DevisTravaux/{id}/decision")]
        Task<DevisTravauxDto> DecideAsync(Guid id, [Body] DevisTravauxDecisionRequest request);

        // GET /api/DevisTravaux/resolve-id?titre=
        [Get("/api/DevisTravaux/resolve-id")]
        Task<DevisTravauxDto> ResolveByTitleAsync(string titre);
    }
}
