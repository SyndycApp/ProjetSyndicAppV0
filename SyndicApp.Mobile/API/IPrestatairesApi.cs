using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Api
{
    public interface IPrestatairesApi
    {
        [Get("/api/Prestataires")]
        Task<List<PrestataireDto>> GetAllAsync(string? search = null);

        [Get("/api/Prestataires/{id}")]
        Task<PrestataireDto> GetByIdAsync(Guid id);

        [Post("/api/Prestataires")]
        Task<PrestataireDto> CreateAsync([Body] PrestataireCreateRequest dto);

        [Put("/api/Prestataires/{id}")]
        Task<PrestataireDto> UpdateAsync(Guid id, [Body] PrestataireUpdateRequest dto);

        [Delete("/api/Prestataires/{id}")]
        Task DeleteAsync(Guid id);
    }
}
