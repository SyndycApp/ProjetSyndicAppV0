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
        [Post("/api/chat/message/audio")]
        Task<MessageDto> SendAudioMessage(
            [AliasAs("ConversationId")] Guid conversationId,
            [AliasAs("AudioFile")] StreamPart audioFile
        );

    }
}
