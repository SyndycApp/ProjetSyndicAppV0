using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api.Communication;
using SyndicApp.Mobile.Models;
using SyndicApp.Mobile.Services.Communication;
using System.Collections.ObjectModel;

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

        // SignalR live messages
        _hub.OnMessageReceived += msg =>
        {
            if (msg.ConversationId == ConversationId)
                Messages.Add(msg);
        };
    }

    // QueryProperty → string obligatoire
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

        var list = await _api.GetMessagesAsync(ConversationId);
        Console.WriteLine($"[DEBUG] App.UserId = {App.UserId}");
        Console.WriteLine($"[DEBUG] Messages count = {Messages.Count}");
        foreach (var m in Messages)
            Console.WriteLine($"MSG => {m.Contenu} / From {m.UserId}");

        Messages.Clear();
        foreach (var msg in list.OrderBy(m => m.CreatedAt))
            Messages.Add(msg);

        Messages = new ObservableCollection<MessageDto>(Messages);

    }


    [RelayCommand]
    public async Task SendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(NewMessage))
            return;

        var req = new SendMessageRequest
        {
            ConversationId = ConversationId,
            Contenu = NewMessage
        };

        var sent = await _api.SendMessageAsync(req);

        Messages.Add(sent);

        NewMessage = string.Empty;
    }
}
