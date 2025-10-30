using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAppelsApi
{
    [Get("/api/Appels")] Task<List<object>> GetAll();
    [Post("/api/Appels")] Task<object> Create([Body] object dto);
    [Get("/api/Appels/{id}")] Task<object> Get(Guid id);
    [Put("/api/Appels/{id}")] Task<object> Update(Guid id, [Body] object dto);
    [Delete("/api/Appels/{id}")] Task<object> Delete(Guid id);
    [Put("/api/Appels/{id}/cloturer")] Task<object> Cloturer(Guid id);
}
