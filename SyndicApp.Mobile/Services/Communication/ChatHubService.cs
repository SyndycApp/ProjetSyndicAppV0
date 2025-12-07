using Microsoft.AspNetCore.SignalR.Client;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Services.Communication
{
    public class ChatHubService
    {
        private HubConnection _hub;
        private readonly string _hubUrl;

        public event Action<MessageDto>? OnMessageReceived;
        public event Action<Guid>? OnUserTyping;

        public ChatHubService(string hubBaseUrl, string token)
        {
            _hubUrl = $"{hubBaseUrl}/chatHub";

            _hub = new HubConnectionBuilder()
                .WithUrl(_hubUrl, options =>
                {
                    options.Headers.Add("Authorization", $"Bearer {token}");
                })
                .WithAutomaticReconnect()
                .Build();

            // Écouter les messages
            _hub.On<MessageDto>("ReceiveMessage", (msg) =>
            {
                OnMessageReceived?.Invoke(msg);
            });

            // Écouter typing
            _hub.On<Guid>("UserTyping", (userId) =>
            {
                OnUserTyping?.Invoke(userId);
            });
        }

        public async Task ConnectAsync()
        {
            if (_hub.State != HubConnectionState.Connected)
                await _hub.StartAsync();
        }

        public Task JoinConversation(Guid conversationId)
        {
            return _hub.InvokeAsync("JoinConversation", conversationId);
        }

        public Task SendMessage(MessageDto msg)
        {
            return _hub.InvokeAsync("SendMessage", msg.ConversationId, msg);
        }

        public Task SendTyping(Guid conversationId)
        {
            return _hub.InvokeAsync("Typing", conversationId, Guid.Parse(App.UserId!));
        }
    }
}
