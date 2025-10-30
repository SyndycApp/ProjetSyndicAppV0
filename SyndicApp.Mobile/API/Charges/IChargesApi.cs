using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IChargesApi
{
    [Get("/api/Charges")] Task<List<object>> GetAll();
    [Post("/api/Charges")] Task<object> Create([Body] object dto);
    [Get("/api/Charges/{id}")] Task<object> Get(Guid id);
    [Put("/api/Charges/{id}")] Task<object> Update(Guid id, [Body] object dto);
    [Delete("/api/Charges/{id}")] Task<object> Delete(Guid id);
}
