using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Common.Messages;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Batiments
{
    public partial class BatimentsListViewModel : ObservableObject, IRecipient<BatimentChangedMessage>
    {
        private readonly IBatimentsApi _api;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private List<BatimentDto> items = new();

        public BatimentsListViewModel(IBatimentsApi api)
        {
            _api = api;

            WeakReferenceMessenger.Default.Register<BatimentChangedMessage>(this,
                async (_, __) => await LoadAsync());
        }

        // ===== CHARGEMENT LISTE =====
        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var list = await _api.GetForCurrentUserAsync();
                Items = list.ToList();
            }
            finally
            {
                IsBusy = false;
            }
        }

        // ===== OUVERTURE DETAILS =====
        [RelayCommand]
        public Task OpenDetailsAsync(Guid id)
            => Shell.Current.GoToAsync($"batiment-details?id={id:D}");

        // ===== OUVERTURE CREATE =====
        [RelayCommand]
        public Task OpenCreateAsync()
            => Shell.Current.GoToAsync("batiment-create");

        public async void Receive(BatimentChangedMessage message) => await LoadAsync();
    }
}
