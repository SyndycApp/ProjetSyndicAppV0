using Microsoft.AspNetCore.SignalR.Client;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Services.Communication
{
    public class ChatHubService
    {
        private readonly HubConnection _hub;

        // =====================
        // EVENTS
        // =====================
        public event Action<MessageDto>? OnMessageReceived;
        public event Action<Guid>? OnUserTyping;
        public event Action<Guid, string, Guid>? OnMessageReacted;

        // =====================
        // STATE
        // =====================
        public bool IsConnected => _hub.State == HubConnectionState.Connected;

        public ChatHubService(string hubBaseUrl, string token)
        {
            _hub = new HubConnectionBuilder()
                .WithUrl($"{hubBaseUrl}/chatHub", options =>
                {
                    options.Headers.Add("Authorization", $"Bearer {token}");
                })
                .WithAutomaticReconnect()
                .Build();

            // =====================
            // 📩 MESSAGE
            // =====================
            _hub.On<MessageDto>("ReceiveMessage", msg =>
            {
                OnMessageReceived?.Invoke(msg);
            });

            // =====================
            // ✍️ TYPING
            // =====================
            _hub.On<Guid>("UserTyping", userId =>
            {
                OnUserTyping?.Invoke(userId);
            });

            // =====================
            // 👍 REACTION
            // =====================
            _hub.On<Guid, string, Guid>(
                "MessageReacted",
                (messageId, emoji, userId) =>
                {
                    OnMessageReacted?.Invoke(messageId, emoji, userId);
                });
        }

        // =====================
        // 🔌 SAFE CONNECTION
        // =====================
        public async Task EnsureConnectedAsync()
        {
            if (_hub.State != HubConnectionState.Connected)
            {
                await _hub.StartAsync();
            }
        }

        // =====================
        // 👥 CONVERSATION
        // =====================
        public async Task JoinConversation(Guid conversationId)
        {
            await EnsureConnectedAsync();
            await _hub.InvokeAsync("JoinConversation", conversationId);
        }

        // =====================
        // 💬 MESSAGE
        // =====================
        public async Task SendMessage(MessageDto msg)
        {
            await EnsureConnectedAsync();
            await _hub.InvokeAsync("SendMessage", msg.ConversationId, msg);
        }

        // =====================
        // ✍️ TYPING
        // =====================
        public async Task SendTyping(Guid conversationId)
        {
            await EnsureConnectedAsync();
            await _hub.InvokeAsync(
                "Typing",
                conversationId,
                Guid.Parse(App.UserId!)
            );
        }

        // =====================
        // 👍 REACTION
        // =====================
        public async Task SendReaction(
            Guid conversationId,
            Guid messageId,
            string emoji)
        {
            await EnsureConnectedAsync();

            await _hub.InvokeAsync(
                "ReactToMessage",
                conversationId,
                messageId,
                emoji,
                Guid.Parse(App.UserId!)
            );
        }
    }
}
