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

        [Post("/api/messages/{conversationId}/read")]
        Task MarkConversationAsReadAsync(Guid conversationId);

        [Post("/api/messages")]
        Task<MessageDto> SendMessageAsync([Body] SendMessageRequest req);

        [Multipart]
        [Post("/api/chat/message/document/{conversationId}")]
        Task<MessageDto> SendDocumentAsync(Guid conversationId, [AliasAs("Document")] StreamPart document);

        [Multipart]
        [Post("/api/chat/message/image/{conversationId}")]
        Task<MessageDto> SendImageAsync(Guid conversationId, [AliasAs("Image")] StreamPart image);

        [Post("/api/chat/message/location")]
        Task<MessageDto> SendLocationAsync([Body] SendLocationDto request);

        [Multipart]
        [Post("/api/chat/message/audio/{conversationId}")]
        Task<MessageDto> SendAudioMessageAsync(Guid conversationId, [AliasAs("AudioFile")] StreamPart audioFile);

        [Post("/api/chat/message/{messageId}/reaction")]
        Task ReactToMessageAsync(Guid messageId, [Body] string emoji);
    }
}
