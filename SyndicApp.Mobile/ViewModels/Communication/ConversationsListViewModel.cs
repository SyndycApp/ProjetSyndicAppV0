using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api.Communication;
using SyndicApp.Mobile.Models;
using System.Collections.ObjectModel;
using static Android.Provider.Telephony.Sms;

namespace SyndicApp.Mobile.ViewModels.Communication
{
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

        [ObservableProperty]
        private bool isLoading;

        [RelayCommand]
        public async Task LoadConversations()
        {
            try
            {
                IsLoading = true;

                var data = await _api.GetConversationsAsync();
                Conversations.Clear();

                foreach (var c in data)
                    Conversations.Add(c);
            }
            catch
            {
                // gérer erreurs
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
