using SyndicApp.Mobile.Models;
using Refit;

public interface IResidencesApi
{
    [Get("/api/Residences")] Task<List<ResidenceDto>> GetAllAsync();
    [Post("/api/Residences")] Task<ResidenceDto> CreateAsync([Body] ResidenceDto dto);
    [Get("/api/Residences/{id}")] Task<ResidenceDto> GetByIdAsync(string id);
    [Put("/api/Residences/{id}")] Task UpdateAsync(string id, [Body] ResidenceDto dto);
    [Delete("/api/Residences/{id}")] Task DeleteAsync(Guid id);
}
