using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api.Communication;
using SyndicApp.Mobile.Models;
using System.Collections.ObjectModel;
using static KotlinX.Serialization.Descriptors.PolymorphicKind;

namespace SyndicApp.Mobile.ViewModels.Communication
{
    public partial class NewConversationViewModel : ObservableObject
    {
        private readonly IChatApi _api;
        private readonly IConversationsApi _conversationsApi;

        public NewConversationViewModel(IChatApi api, IConversationsApi conversationsApi)
        {
            _api = api;
            _conversationsApi = conversationsApi;
            Users = new();
        }

        [ObservableProperty]
        private ObservableCollection<ChatUserDto> users;

        [RelayCommand]
        public async Task LoadAsync()
        {
            // 1️⃣ récupérer toutes les conversations existantes
            var conversations = await _conversationsApi.GetConversationsAsync();

            var myId = Guid.Parse(Preferences.Get("userId", ""));

            // 2️⃣ récupérer les IDs déjà en conversation
            var existingUserIds = conversations
                .SelectMany(c => c.Participants)
                .Where(p => p.UserId != myId)
                .Select(p => p.UserId)
                .ToHashSet();

            // 3️⃣ récupérer tous les utilisateurs
            var allUsers = await _api.GetChatUsers();

            // 4️⃣ filtrer ceux déjà en conversation
            var filtered = allUsers
                .Where(u => !existingUserIds.Contains(u.UserId))
                .ToList();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Users.Clear();
                foreach (var u in filtered)
                    Users.Add(u);
            });
        }

        [RelayCommand]
        public async Task OpenAsync(ChatUserDto user)
        {
            var req = new OpenConversationRequest
            {
                OtherUserId = user.UserId
            };

            var open = await _api.OpenConversation(req);

            await Shell.Current.GoToAsync(
        $"../chat?conversationId={open.ConversationId}&name={Uri.EscapeDataString(user.NomComplet)}"
    );
        }
    }
}
