using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ILotsApi
{
	[Get("/api/Lots")] Task<List<object>> GetAll();
	[Post("/api/Lots")] Task<object> Create([Body] object dto);
	[Get("/api/Lots/{id}")] Task<object> Get(Guid id);
	[Put("/api/Lots/{id}")] Task<object> Update(Guid id, [Body] object dto);
	[Delete("/api/Lots/{id}")] Task<object> Delete(Guid id);
	[Get("/api/Lots/by-residence/{residenceId}")] Task<List<object>> ByResidence(Guid residenceId);
}
