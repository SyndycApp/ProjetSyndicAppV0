using Refit;
using SyndicApp.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.Api
{
    public interface ILotsApi
    {
        [Get("/api/Lots")]
        Task<List<LotDto>> GetAllAsync();

        [Get("/api/Lots/by-residence/{residenceId}")]
        Task<List<LotDto>> GetByResidenceAsync(Guid residenceId);

        [Get("/api/Lots/{id}")]
        Task<LotDto> GetByIdAsync(Guid id);

        [Post("/api/Lots")]
        Task<IdResponse> CreateAsync([Body] CreateLotDto dto);

        [Put("/api/Lots/{id}")]
        Task UpdateAsync(Guid id, [Body] UpdateLotDto dto);

        [Delete("/api/Lots/{id}")]
        Task DeleteAsync(Guid id);

        [Get("/api/Lots/resolve-id")]
        Task<IdResponse> ResolveIdAsync([Query] string numeroLot, [Query] string type);

        [Get("/api/Lots/resolve-id")]
        Task<List<LotResolveItem>> ResolveManyAsync([Query] string? numeroLot = null,[Query] string? type = null);
    }

    public class IdResponse { public Guid Id { get; set; } }
}
