using SyndicApp.Mobile.Api.Communication;
using SyndicApp.Mobile.Services.Communication;
using System.Collections.ObjectModel;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Communication;

[QueryProperty(nameof(ConversationIdString), "conversationId")]
public partial class ChatViewModel : ObservableObject
{
    private readonly IMessagesApi _api;
    private readonly ChatHubService _hub;

    public ChatViewModel(IMessagesApi api, ChatHubService hub)
    {
        _api = api;
        _hub = hub;

        Messages = new ObservableCollection<MessageDto>();
    }

    [ObservableProperty]
    private string conversationIdString;

    public Guid ConversationId => Guid.Parse(ConversationIdString);

    [ObservableProperty]
    private ObservableCollection<MessageDto> messages;

    [ObservableProperty]
    private string newMessage;

    [RelayCommand]
    public async Task LoadMessagesAsync()
    {
        if (string.IsNullOrWhiteSpace(ConversationIdString))
            return;

        var page = await _api.GetMessagesPagedAsync(ConversationId, 1, 50);

        Messages.Clear();
        foreach (var msg in page.Messages.OrderBy(m => m.CreatedAt))
            Messages.Add(msg);
    }
}
