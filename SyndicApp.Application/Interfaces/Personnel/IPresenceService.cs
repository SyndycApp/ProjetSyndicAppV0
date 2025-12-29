using SyndicApp.Application.DTOs.Personnel;

namespace SyndicApp.Application.Interfaces.Personnel;

public interface IPresenceService
{
    Task StartAsync(Guid userId, StartPresenceDto dto);
    Task EndAsync(Guid userId);
    Task<IReadOnlyList<PresenceDto>> GetMyHistoryAsync(Guid userId);
}
