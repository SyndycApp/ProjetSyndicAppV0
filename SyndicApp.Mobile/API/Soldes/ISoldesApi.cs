using Refit;
using System;
using System.Threading.Tasks;

public interface ISoldesApi
{
    [Get("/api/lots/{lotId}/solde")] Task<object> SoldeLot(Guid lotId);
    [Get("/api/residences/{residenceId}/solde")] Task<object> SoldeResidence(Guid residenceId);
}
