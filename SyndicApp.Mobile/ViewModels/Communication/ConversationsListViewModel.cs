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
        Console.WriteLine("=== 📌 LoadConversationsAsync START ===");

        var data = await _api.GetConversationsAsync();

        Console.WriteLine($"🔵 API Returned: {data?.Count()} conversations");

        int index = 0;
        foreach (var c in data)
        {
            Console.WriteLine($"   → #{index++} {c.OtherParticipant?.NomComplet} / DernierMsg: {c.DernierMessage?.Contenu}");
        }

        // Mise à jour UI
        Conversations.Clear();
        foreach (var c in data)
            Conversations.Add(c);

        Console.WriteLine($"🟢 Conversations ObservableCollection Count = {Conversations.Count}");
        Console.WriteLine("=== 📌 LoadConversationsAsync END ===");
    }

}
