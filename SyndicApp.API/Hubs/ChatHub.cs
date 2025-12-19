using Microsoft.AspNetCore.SignalR;
using SyndicApp.Application.DTOs.Communication;
using SyndicApp.Application.Interfaces.Communication;

namespace SyndicApp.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;

        // 🔹 Injection du service métier (OBLIGATOIRE)
        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // =====================================================
        // 💬 MESSAGE
        // =====================================================
        public async Task SendMessage(Guid conversationId, MessageDto message)
        {
            await Clients.Group(conversationId.ToString())
                         .SendAsync("ReceiveMessage", message);
        }

        // =====================================================
        // 👥 JOIN CONVERSATION
        // =====================================================
        public async Task JoinConversation(Guid conversationId)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                conversationId.ToString()
            );
        }

        // =====================================================
        // 👍 REACTION (FIX DÉFINITIF)
        // =====================================================
        public async Task ReactToMessage(
            Guid conversationId,
            Guid messageId,
            string emoji,
            Guid userId)
        {
            // ✅ 1️⃣ PERSISTENCE EN BASE
            await _messageService.AddReactionAsync(
                messageId,
                userId,
                emoji
            );

            // ✅ 2️⃣ BROADCAST TEMPS RÉEL
            await Clients.Group(conversationId.ToString())
                .SendAsync(
                    "MessageReacted",
                    messageId,
                    emoji,
                    userId
                );
        }

        // =====================================================
        // ✍️ TYPING
        // =====================================================
        public async Task Typing(Guid conversationId, Guid userId)
        {
            await Clients.Group(conversationId.ToString())
                         .SendAsync("UserTyping", userId);
        }
    }
}
