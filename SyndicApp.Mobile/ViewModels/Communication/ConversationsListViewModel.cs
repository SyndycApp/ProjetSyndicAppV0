using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api.Communication;
using SyndicApp.Mobile.Models;
using System.Collections.ObjectModel;

namespace SyndicApp.Mobile.ViewModels.Communication
{
    public partial class ConversationsListViewModel : ObservableObject
    {
        private readonly IConversationsApi _api;

        public ConversationsListViewModel(IConversationsApi api)
        {
            _api = api;
            Conversations = new ObservableCollection<ConversationItemViewModel>();
        }

        [ObservableProperty]
        private ObservableCollection<ConversationItemViewModel> conversations;

        private string GetOtherName(ConversationDto conv)
        {
            var myId = Preferences.Get("userId", "").Trim();

            var other = conv.Participants
                .FirstOrDefault(p => p.UserId.ToString().Trim() != myId);

            return other?.NomComplet ?? "Utilisateur";
        }


        [RelayCommand]
        private async Task NewConversation()
        {
            await Shell.Current.GoToAsync("new-conversation");
        }

        [RelayCommand]
        public async Task LoadConversationsAsync()
        {
            try
            {
                var list = await _api.GetConversationsAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Conversations.Clear();

                    foreach (var c in list)
                    {
                        var name = GetOtherName(c);
                        Conversations.Add(new ConversationItemViewModel(c, name));
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERROR LoadConversationsAsync : " + ex);
            }
        }

    }
}
