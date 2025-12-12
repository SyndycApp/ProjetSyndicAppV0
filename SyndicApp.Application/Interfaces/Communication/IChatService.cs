using SyndicApp.Application.DTOs.Communication;

namespace SyndicApp.Application.Interfaces.Communication
{
    public interface IChatService
    {
        Task<List<UserChatDto>> GetAllUsersExceptAsync(Guid userId);

        Task<Guid> OpenOrCreateConversationAsync(Guid currentUserId, Guid otherUserId);
    }
}
