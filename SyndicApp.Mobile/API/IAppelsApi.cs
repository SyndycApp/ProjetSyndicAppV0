using Refit;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Api
{
    public interface IAppelsApi
    {
        [Get("/api/Appels")]
        Task<List<AppelDeFondsDto>> GetAllAsync();

        [Get("/api/Appels/{id}")]
        Task<AppelDeFondsDto> GetByIdAsync(string id);                 // ← string

        [Post("/api/Appels")]
        Task<AppelDeFondsDto> CreateAsync([Body] AppelDeFondsDto body);

        [Put("/api/Appels/{id}")]
        Task<AppelDeFondsDto> UpdateAsync(string id, [Body] AppelDeFondsDto body); // ← string

        [Delete("/api/Appels/{id}")]
        Task DeleteAsync(string id);                                    // ← string

        [Put("/api/Appels/{id}/cloturer")]
        Task<AppelDeFondsDto> CloturerAsync(string id);                 // ← string
    }
}
