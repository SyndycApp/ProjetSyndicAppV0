using SyndicApp.Application.DTOs.Assemblees;

namespace SyndicApp.Application.Interfaces.Assemblees
{
    public interface IDecisionService
    {
        Task<DecisionDto> CreerDecisionAsync(Guid resolutionId);
        Task<List<DecisionDto>> GetDecisionsByAssembleeAsync(Guid assembleeId);
    }
}
