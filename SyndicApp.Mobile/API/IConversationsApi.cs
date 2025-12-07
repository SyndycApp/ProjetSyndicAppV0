using Refit;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Api.Communication
{
    public interface IConversationsApi
    {
        [Get("/api/messages/conversations")]
        Task<List<ConversationDto>> GetConversationsAsync();

        [Post("/api/messages/conversations")]
        Task<ConversationDto> CreateConversationAsync([Body] CreateConversationRequest req);
    }
}
