using SyndicApp.Application.DTOs.Personnel;

namespace SyndicApp.Application.Interfaces.Personnel
{
    public interface IPresenceMissionService
    {
        Task StartAsync(Guid userId, StartMissionPresenceDto dto);
        Task EndAsync(Guid userId, EndMissionPresenceDto dto);

        Task<IReadOnlyList<TempsTravailJourDto>> GetTempsTravailParJourAsync(Guid employeUserId);

        Task<IReadOnlyList<PresenceMissionDto>> GetByMission(Guid missionId);
    }
}
