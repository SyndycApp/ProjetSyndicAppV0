using Refit;
using SyndicApp.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.Api
{
    public interface IPaiementsApi
    {
        // GET /api/Paiements
        [Get("/api/Paiements")]
        Task<List<PaiementDto>> GetAllAsync();

        [Get("/api/Paiements/{id}")]
        Task<PaiementDto> GetByIdAsync(Guid id);

        // POST /api/Paiements
        [Post("/api/Paiements")]
        Task<PaiementDto> CreateAsync([Body] PaiementCreateRequest request);

        [Get("/api/Paiements/by-appel/{appelId}")]
        Task<List<PaiementDto>> GetByAppelIdAsync(string appelId);
        
    }
}
