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
    }
}
