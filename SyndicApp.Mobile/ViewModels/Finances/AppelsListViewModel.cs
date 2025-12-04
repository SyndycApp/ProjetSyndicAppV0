using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Finances
{
    public partial class AppelsListViewModel : ObservableObject
    {
        private readonly IAppelsApi _api;
        private readonly IResidencesApi _residencesApi;

        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private List<AppelDeFondsDto> appels = new();
        [ObservableProperty] private bool isSyndic;

        public AppelsListViewModel(IAppelsApi api, IResidencesApi residencesApi)
        {
            _api = api;
            _residencesApi = residencesApi;

            IsSyndic = Preferences.Get("user_role", "").ToLowerInvariant().Contains("syndic");
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var list = await _api.GetAllAsync() ?? new();
                Appels = list;

                var residences = await _residencesApi.GetAllAsync() ?? new();
                var lookup = residences.ToDictionary(r => r.Id.ToString(), r => r.Nom ?? string.Empty);

                foreach (var a in list)
                {
                    if (lookup.TryGetValue(a.ResidenceId.ToString(), out var nom))
                        a.ResidenceNom = nom;
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task OpenCreateAsync()
        {
            if (!IsSyndic)
            {
                await Shell.Current.DisplayAlert("Accès refusé", "Seul le syndic peut créer un appel.", "OK");
                return;
            }

            await Shell.Current.GoToAsync("appel-create");
        }

        [RelayCommand]
        public async Task OpenDetailsAsync(AppelDeFondsDto dto)
            => await Shell.Current.GoToAsync($"appel-details?id={dto.Id}");
    }
}
