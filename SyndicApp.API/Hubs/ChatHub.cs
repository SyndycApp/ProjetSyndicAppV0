using Microsoft.AspNetCore.SignalR;
using SyndicApp.Application.DTOs.Communication;

namespace SyndicApp.API.Hubs
{
    public class ChatHub : Hub
    {
        // Envoyer un message à tous les membres de la conversation
        public async Task SendMessage(Guid conversationId, MessageDto message)
        {
            await Clients.Group(conversationId.ToString())
                         .SendAsync("ReceiveMessage", message);
        }

        // Rejoindre un groupe = une conversation
        public async Task JoinConversation(Guid conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
        }

        public async Task ReactToMessage(Guid conversationId, Guid messageId, string emoji, Guid userId)
        {
            await Clients.Group(conversationId.ToString())
                .SendAsync("MessageReacted", messageId, emoji, userId);
        }

        // Indicateur "en train d'écrire..."
        public async Task Typing(Guid conversationId, Guid userId)
        {
            await Clients.Group(conversationId.ToString())
                         .SendAsync("UserTyping", userId);
        }
    }
}
