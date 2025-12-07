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

        // Indicateur "en train d'écrire..."
        public async Task Typing(Guid conversationId, Guid userId)
        {
            await Clients.Group(conversationId.ToString())
                         .SendAsync("UserTyping", userId);
        }
    }
}
