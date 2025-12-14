using Refit;
using SyndicApp.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.Api.Communication
{
    public interface IChatApi
    {
        [Get("/api/chat/users")]
        Task<List<ChatUserDto>> GetChatUsers();

        [Post("/api/chat/open")]
        Task<OpenConversationResponse> OpenConversation([Body] OpenConversationRequest req);

        [Get("/api/chat/{conversationId}/messages")]
        Task<List<MessageDto>> GetMessages(Guid conversationId);

        [Post("/api/chat/message")]
        Task<MessageDto> SendTextMessage([Body] SendMessageRequest request);

        [Multipart]
        [Post("/api/chat/message/audio/{conversationId}")]
        Task<MessageDto> SendAudioMessage(Guid conversationId, [AliasAs("AudioFile")] StreamPart audioFile);

        [Multipart]
        [Post("/api/chat/message/document/{conversationId}")]
        Task<MessageDto> SendDocumentAsync(Guid conversationId,[AliasAs("Document")] StreamPart document);

        [Multipart]
        [Post("/api/chat/message/image/{conversationId}")]
        Task<MessageDto> SendImageAsync(Guid conversationId,[AliasAs("Image")] StreamPart image);

        [Post("/api/chat/message/location")]
        Task<MessageDto> SendLocationAsync([Body] SendLocationDto request);
    }
}
