using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IInterventionsApi
{
    [Post("/api/Interventions")] Task<object> Create([Body] object dto);
    [Get("/api/Interventions")] Task<List<object>> GetAll();
    [Get("/api/Interventions/{id}")] Task<object> Get(Guid id);
    [Delete("/api/Interventions/{id}")] Task<object> Delete(Guid id);
    [Get("/api/Interventions/by-residence/{residenceId}")] Task<List<object>> ByResidence(Guid residenceId);
    [Put("/api/Interventions/{id}/status")] Task<object> UpdateStatus(Guid id, [Body] object dto);
}
