using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPaiementsApi
{
    [Get("/api/Paiements")] Task<List<object>> GetAll();
    [Post("/api/Paiements")] Task<object> Create([Body] object dto);
    [Get("/api/Paiements/{id}")] Task<object> Get(Guid id);
}
