using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api.Communication;
using SyndicApp.Mobile.Models;
using SyndicApp.Mobile.Services.Communication;
using System.Collections.ObjectModel;

namespace SyndicApp.Mobile.ViewModels.Communication
{
    public partial class ChatViewModel : ObservableObject
    {
        private readonly IMessagesApi _api;
        private readonly ChatHubService _hub;

        public ChatViewModel(
            IMessagesApi api,
            ChatHubService hub)
        {
            _api = api;
            _hub = hub;

            Messages = new ObservableCollection<MessageDto>();

            _hub.OnMessageReceived += (msg) =>
            {
                if (msg.ConversationId == ConversationId)
                    Messages.Add(msg);
            };
        }

        public Guid ConversationId { get; set; }

        [ObservableProperty]
        private ObservableCollection<MessageDto> messages;

        [ObservableProperty]
        private string newMessage = string.Empty;

        [RelayCommand]
        public async Task LoadMessages()
        {
            var page = await _api.GetMessagesPagedAsync(ConversationId, 1, 50);

            Messages.Clear();
            foreach (var msg in page.Messages.OrderBy(m => m.CreatedAt))
                Messages.Add(msg);

            await _hub.JoinConversation(ConversationId);
        }

        [RelayCommand]
        public async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(NewMessage))
                return;

            var req = new SendMessageRequest
            {
                ConversationId = ConversationId,
                Contenu = NewMessage
            };

            var sent = await _api.SendMessageAsync(req);
            NewMessage = string.Empty;
        }
    }
}
