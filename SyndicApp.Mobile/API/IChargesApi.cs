using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Api
{
    public interface IChargesApi
    {
        [Get("/api/Charges")]
        Task<List<ChargeDto>> GetAllAsync();

        [Get("/api/Charges/{id}")]
        Task<ChargeDto> GetByIdAsync(Guid id);

        [Post("/api/Charges")]
        Task<ChargeDto> CreateAsync([Body] ChargeCreateRequest request);

        [Put("/api/Charges/{id}")]
        Task<ChargeDto> UpdateAsync(Guid id, [Body] ChargeCreateRequest request);

        [Delete("/api/Charges/{id}")]
        Task DeleteAsync(Guid id);
    }
}
