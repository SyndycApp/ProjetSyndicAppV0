using Refit;
using Refit;
using SyndicApp.Mobile.Models;

public interface IBatimentsApi
{
    [Get("/api/Batiments")]
    Task<List<BatimentDto>> GetAllAsync();

    [Post("/api/Batiments")]
    Task<BatimentDto> CreateAsync([Body] BatimentCreateDto dto);

    [Get("/api/Batiments/{id}")]
    Task<BatimentDto> GetByIdAsync(Guid id);

    [Put("/api/Batiments/{id}")]
    Task UpdateAsync(Guid id, [Body] BatimentUpdateDto dto);

    [Delete("/api/Batiments/{id}")]
    Task DeleteAsync(Guid id);

    [Get("/api/Batiments/resolve-id")]
    Task<Guid?> ResolveIdAsync([Query] string name);
}

