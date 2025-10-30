using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IIncidentsApi
{
    [Post("/api/Incidents")] Task<object> Create([Body] object dto);
    [Get("/api/Incidents")] Task<List<object>> GetAll();
    [Get("/api/Incidents/{id}")] Task<object> Get(Guid id);
    [Put("/api/Incidents/{id}")] Task<object> Update(Guid id, [Body] object dto);
    [Delete("/api/Incidents/{id}")] Task<object> Delete(Guid id);
    [Get("/api/Incidents/by-residence/{residenceId}")] Task<List<object>> ByResidence(Guid residenceId);
    [Put("/api/Incidents/{id}/status")] Task<object> UpdateStatus(Guid id, [Body] object dto);
}
