using Refit;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Api
{
    public interface IAppelsApi
    {
        [Get("/api/Appels")]
        Task<List<AppelDeFondsDto>> GetAllAsync();

        [Get("/api/Appels/{id}")]
        Task<AppelDeFondsDto> GetByIdAsync(string id);                

        [Post("/api/Appels")]
        Task<AppelDeFondsDto> CreateAsync([Body] AppelDeFondsDto body);

        [Put("/api/Appels/{id}")]
        Task UpdateAsync(string id, [Body] AppelDeFondsDto body);

        // 204 → pas de body
        [Put("/api/Appels/{id}/cloturer")]
        Task CloturerAsync(string id);

        [Delete("/api/Appels/{id}")]
        Task DeleteAsync(string id);

        [Get("/api/Appels/{id}/description")]
        Task<string> GetDescriptionAsync(string id);
    }
}
