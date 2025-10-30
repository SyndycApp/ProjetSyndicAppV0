using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBatimentsApi
{
    [Get("/api/Batiments")] Task<List<object>> GetAll();
    [Post("/api/Batiments")] Task<object> Create([Body] object dto);
    [Get("/api/Batiments/{id}")] Task<object> Get(Guid id);
    [Put("/api/Batiments/{id}")] Task<object> Update(Guid id, [Body] object dto);
    [Delete("/api/Batiments/{id}")] Task<object> Delete(Guid id);
    [Get("/api/Batiments/by-residence/{residenceId}")] Task<List<object>> ByResidence(Guid residenceId);
}
