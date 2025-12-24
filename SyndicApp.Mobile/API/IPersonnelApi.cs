using Refit;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Api;

public interface IPersonnelApi
{
    [Get("/api/personnel/interne")]
    Task<List<PersonnelLookupDto>> GetPersonnelInterneAsync();

    // (prévu pour la suite backend)
    [Get("/api/planning/{userId}")]
    Task<List<PlanningDto>> GetPlanningAsync(Guid userId);

    [Get("/api/presence/{userId}")]
    Task<List<PresenceDto>> GetPresencesAsync(Guid userId);
}
