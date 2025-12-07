using Refit;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Api.Communication
{
    public interface IMessagesApi
    {
        [Get("/api/messages/{conversationId}")]
        Task<List<MessageDto>> GetMessagesAsync(Guid conversationId);

        [Get("/api/messages/{conversationId}/messages")]
        Task<PagedMessagesDto> GetMessagesPagedAsync(
            Guid conversationId,
            int page,
            int pageSize
        );

        [Post("/api/messages")]
        Task<MessageDto> SendMessageAsync([Body] SendMessageRequest req);
    }
}
