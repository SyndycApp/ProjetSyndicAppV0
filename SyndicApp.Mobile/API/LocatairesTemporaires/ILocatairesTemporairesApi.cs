using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ILocatairesTemporairesApi
{
    [Get("/api/LocatairesTemporaires")] Task<List<object>> GetAll();
    [Post("/api/LocatairesTemporaires")] Task<object> Create([Body] object dto);
    [Put("/api/LocatairesTemporaires/{id}")] Task<object> Update(Guid id, [Body] object dto);
    [Delete("/api/LocatairesTemporaires/{id}")] Task<object> Delete(Guid id);
    [Get("/api/LocatairesTemporaires/by-lot/{lotId}")] Task<List<object>> ByLot(Guid lotId);
}
