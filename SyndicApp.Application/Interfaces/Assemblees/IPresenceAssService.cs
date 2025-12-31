using SyndicApp.Application.DTOs.Assemblees;

namespace SyndicApp.Application.Interfaces.Assemblees
{
    public interface IPresenceAssService
    {
        Task EnregistrerPresenceAsync(Guid userId, PresenceAssDto dto);
    }
}
