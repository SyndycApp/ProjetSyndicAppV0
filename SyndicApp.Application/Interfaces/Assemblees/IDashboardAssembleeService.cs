using SyndicApp.Application.DTOs.Assemblees;

namespace SyndicApp.Application.Interfaces.Assemblees;

public interface IDashboardAssembleeService
{
    Task<DashboardAgComparatifDto> GetComparatifAsync(Guid residenceId, int annee);

    Task<List<ParticipationParAgDto>> GetParticipationParAgAsync(Guid residenceId, int annee);

    Task<RepartitionVotesDto> GetRepartitionVotesAsync(Guid assembleeId);
}
