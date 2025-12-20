using SyndicApp.Application.DTOs.AppelVocal;

namespace SyndicApp.Application.Interfaces.AppelVocal
{
    public interface ICallService
    {
        Task<CallDto> StartCallAsync(Guid callerId, Guid receiverId);
        Task AcceptCallAsync(Guid callId);
        Task EndCallAsync(Guid callId);

        Task<IEnumerable<CallDto>> GetHistoryAsync(Guid userId);
        Task<IEnumerable<CallDto>> GetMissedCallsAsync(Guid userId);

        Task<CallDto?> GetByIdAsync(Guid callId);
    }
}
