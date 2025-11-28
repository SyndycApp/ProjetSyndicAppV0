using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Common.Messages;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Residences
{
    public partial class ResidencesListViewModel : ObservableObject, IRecipient<ResidenceChangedMessage>
    {
        private readonly IResidencesApi _api;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private List<ResidenceDto> residences = new();

        public IAsyncRelayCommand LoadAsyncCommand { get; }
        public IAsyncRelayCommand<Guid> OpenDetailsAsyncCommand { get; }
        public IAsyncRelayCommand OpenCreateAsyncCommand { get; }

        public ResidencesListViewModel(IResidencesApi api)
        {
            _api = api;

            LoadAsyncCommand = new AsyncRelayCommand(LoadAsync);
            OpenDetailsAsyncCommand = new AsyncRelayCommand<Guid>(OpenDetailsAsync);
            OpenCreateAsyncCommand = new AsyncRelayCommand(OpenCreateAsync);

            // 🔁 rechargement quand une résidence ou un bâtiment change
            WeakReferenceMessenger.Default.Register<BatimentChangedMessage>(this,
                async (_, __) => await LoadAsync());

            WeakReferenceMessenger.Default.Register<ResidenceChangedMessage>(this,
                async (_, __) => await LoadAsync());

            // ✅ CHARGER DÈS LA CRÉATION DU VM
            _ = LoadAsync();
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var list = await _api.GetForCurrentUserAsync();
                Residences = list.ToList();
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public Task OpenDetailsAsync(Guid id)
            => Shell.Current.GoToAsync($"residence-details?id={id:D}");

        [RelayCommand]
        public Task OpenCreateAsync()
            => Shell.Current.GoToAsync("residence-create");

        public async void Receive(ResidenceChangedMessage message) => await LoadAsync();
    }
}
