using Refit;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Api;

public interface IResidencesApi
{
    [Get("/api/Residences")]
    Task<List<ResidenceDto>> GetAllAsync();

    [Post("/api/Residences")]
    Task<ResidenceDto> CreateAsync([Body] ResidenceCreateDto dto);

    [Get("/api/Residences/{id}")]
    Task<ResidenceDto> GetByIdAsync(Guid id);

    [Put("/api/Residences/{id}")]
    Task<ResidenceDto> UpdateAsync(Guid id, [Body] ResidenceUpdateDto dto);

    [Delete("/api/Residences/{id}")]
    Task<ApiOkDto> DeleteAsync(Guid id);

    [Get("/api/Residences/{id}/details")]
    Task<ResidenceDetailsDto> GetDetailsAsync(Guid id);
}
