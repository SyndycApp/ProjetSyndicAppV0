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


        // ✔ Le backend attend CreateAppelDeFondsDto
        [Post("/api/Appels")]
        Task<CreateResultDto> CreateAsync([Body] CreateAppelDeFondsRequest body);


        // ✔ Le backend attend UpdateAppelDeFondsDto
        [Put("/api/Appels/{id}")]
        Task UpdateAsync(string id, [Body] UpdateAppelDeFondsRequest body);


        // 204 OK
        [Put("/api/Appels/{id}/cloturer")]
        Task CloturerAsync(string id);

        [Delete("/api/Appels/{id}")]
        Task DeleteAsync(string id);

        [Get("/api/Appels/{id}/description")]
        Task<string> GetDescriptionAsync(string id);
    }
}

