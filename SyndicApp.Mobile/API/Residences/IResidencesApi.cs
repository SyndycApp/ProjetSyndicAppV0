using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IResidencesApi
{
    [Get("/api/Residences")] Task<List<object>> GetAll();
    [Post("/api/Residences")] Task<object> Create([Body] object dto);
    [Get("/api/Residences/{id}")] Task<object> Get(Guid id);
    [Put("/api/Residences/{id}")] Task<object> Update(Guid id, [Body] object dto);
    [Delete("/api/Residences/{id}")] Task<object> Delete(Guid id);
    [Get("/api/Residences/{id}/lots")] Task<List<object>> GetLots(Guid id);
    [Get("/api/Residences/{id}/occupants")] Task<List<object>> GetOccupants(Guid id);
    [Get("/api/Residences/{id}/details")] Task<object> GetDetails(Guid id);
}
