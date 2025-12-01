using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
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

            var role = Preferences.Get("user_role", string.Empty)?.ToLowerInvariant() ?? string.Empty;
            IsSyndic = role.Contains("syndic");
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var list = await _api.GetAllAsync() ?? new List<AppelDeFondsDto>();
                Appels = list;

                var residences = await _residencesApi.GetAllAsync() ?? new List<ResidenceDto>();
                var lookup = residences.ToDictionary(
                    r => r.Id.ToString(),
                    r => r.Nom ?? string.Empty);

                foreach (var a in list)
                {
                    if (!string.IsNullOrWhiteSpace(a.ResidenceId)
                        && lookup.TryGetValue(a.ResidenceId, out var nom))
                    {
                        a.ResidenceNom = nom;
                    }
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
                await Shell.Current.DisplayAlert("Accès restreint", "Seul le syndic peut créer un appel de fonds.", "OK");
                return;
            }

            await Shell.Current.GoToAsync("appel-create");
        }

        [RelayCommand]
        public async Task OpenDetailsAsync(AppelDeFondsDto a)
            => await Shell.Current.GoToAsync($"appel-details?id={a.Id}");
    }
}
