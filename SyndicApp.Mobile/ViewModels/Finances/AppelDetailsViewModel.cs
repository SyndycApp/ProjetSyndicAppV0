using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Finances
{
    [QueryProperty(nameof(Id), "id")]
    public partial class AppelDetailsViewModel : ObservableObject
    {
        private readonly IAppelsApi _api;

        [ObservableProperty] private string id = string.Empty;
        [ObservableProperty] private AppelDeFondsDto? appel;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private bool isSyndic;

        public AppelDetailsViewModel(IAppelsApi api)
        {
            _api = api;

            var role = Preferences.Get("user_role", string.Empty)?.ToLowerInvariant() ?? string.Empty;
            IsSyndic = role.Contains("syndic");
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy || string.IsNullOrWhiteSpace(Id)) return;

            try
            {
                IsBusy = true;
                Appel = await _api.GetByIdAsync(Id);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task EditAsync()
        {
            if (!IsSyndic)
            {
                await Shell.Current.DisplayAlert("Accès restreint", "Seul le syndic peut modifier un appel de fonds.", "OK");
                return;
            }

            await Shell.Current.GoToAsync($"appel-edit?id={Id}");
        }

        [RelayCommand]
        public async Task CloturerAsync()
        {
            if (IsBusy || string.IsNullOrWhiteSpace(Id)) return;

            if (!IsSyndic)
            {
                await Shell.Current.DisplayAlert("Accès restreint", "Seul le syndic peut clôturer un appel de fonds.", "OK");
                return;
            }

            try
            {
                IsBusy = true;
                await _api.CloturerAsync(Id);
                Appel = await _api.GetByIdAsync(Id);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task DeleteAsync()
        {
            if (string.IsNullOrWhiteSpace(Id)) return;

            if (!IsSyndic)
            {
                await Shell.Current.DisplayAlert("Accès restreint", "Seul le syndic peut supprimer un appel de fonds.", "OK");
                return;
            }

            var ok = await Shell.Current.DisplayAlert("Suppression", "Supprimer cet appel ?", "Oui", "Non");
            if (!ok) return;

            await _api.DeleteAsync(Id);
            await Shell.Current.GoToAsync("//appels");
        }
    }
}
