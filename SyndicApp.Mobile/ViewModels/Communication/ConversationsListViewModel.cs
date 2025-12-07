using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api.Communication;
using SyndicApp.Mobile.Models;
using System.Collections.ObjectModel;

namespace SyndicApp.Mobile.ViewModels.Communication;

public partial class ConversationsListViewModel : ObservableObject
{
    private readonly IConversationsApi _api;

    public ConversationsListViewModel(IConversationsApi api)
    {
        _api = api;
        Conversations = new ObservableCollection<ConversationDto>();
    }

    [ObservableProperty]
    private ObservableCollection<ConversationDto> conversations;

    [RelayCommand]
    public async Task LoadConversationsAsync()
    {
        var data = await _api.GetConversationsAsync();
        Conversations.Clear();
        foreach (var c in data)
            Conversations.Add(c);
    }

    [RelayCommand]
    public async Task ItemTappedAsync(ConversationDto item)
    {
        if (item == null) return;

        await Shell.Current.GoToAsync($"chat?conversationId={item.Id}");
    }
}
