using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAffectationsLotsApi
{
    [Get("/api/AffectationsLots")] Task<List<object>> GetAll();
    [Post("/api/AffectationsLots")] Task<object> Create([Body] object dto);
    [Get("/api/AffectationsLots/{id}")] Task<object> Get(Guid id);
    [Put("/api/AffectationsLots/{id}")] Task<object> Update(Guid id, [Body] object dto);
    [Delete("/api/AffectationsLots/{id}")] Task<object> Delete(Guid id);

    [Get("/api/AffectationsLots/by-lot/{lotId}")] Task<List<object>> ByLot(Guid lotId);
    [Get("/api/AffectationsLots/by-user/{userId}")] Task<List<object>> ByUser(Guid userId);
    [Put("/api/AffectationsLots/{id}/cloturer")] Task<object> Cloturer(Guid id);
    [Get("/api/AffectationsLots/lot/{lotId}/historique")] Task<List<object>> HistoriqueLot(Guid lotId);
    [Get("/api/AffectationsLots/lot/{lotId}/occupant-actuel")] Task<object> OccupantActuelLot(Guid lotId);
    [Post("/api/AffectationsLots/assigner-locataire")] Task<object> AssignerLocataire([Body] object dto);
    [Put("/api/AffectationsLots/{id}/changer-statut")] Task<object> ChangerStatut(Guid id, [Body] object dto);
    [Get("/api/lots/{lotId}/occupant-actuel")] Task<object> OccupantActuel(Guid lotId);
}
