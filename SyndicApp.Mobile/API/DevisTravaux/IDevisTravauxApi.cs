using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IDevisTravauxApi
{
    [Post("/api/DevisTravaux")] Task<object> Create([Body] object dto);
    [Get("/api/DevisTravaux")] Task<List<object>> GetAll();
    [Get("/api/DevisTravaux/{id}")] Task<object> Get(Guid id);
    [Delete("/api/DevisTravaux/{id}")] Task<object> Delete(Guid id);
    [Get("/api/DevisTravaux/by-residence/{residenceId}")] Task<List<object>> ByResidence(Guid residenceId);
    [Put("/api/DevisTravaux/{id}/decision")] Task<object> Decide(Guid id, [Body] object dto);
}
