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

        [ObservableProperty]
        private string id = string.Empty;

        [ObservableProperty]
        private AppelDeFondsDto? appel;

        [ObservableProperty]
        private double progress;

        // 🔥 États
        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool isSyndic;

        public AppelDetailsViewModel(IAppelsApi api)
        {
            _api = api;

            var role = Preferences.Get("user_role", string.Empty)?.ToLowerInvariant() ?? "";
            IsSyndic = role.Contains("syndic");
        }

        // 🔥 Méthode générée automatiquement par CommunityToolkit : OnAppelChanged
        // 🔥 Calcul automatique du pourcentage quand Appel change
        partial void OnAppelChanged(AppelDeFondsDto? value)
        {
            if (value == null)
            {
                Progress = 0;
                return;
            }

            // 🔥 Empêche division par zéro + convertit en double
            if (value.MontantTotal <= 0)
            {
                Progress = 0;
                return;
            }

            Progress = (double)value.MontantPaye / (double)value.MontantTotal;
        }


        // 🔥 Charger les données
        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy || string.IsNullOrWhiteSpace(Id)) return;

            try
            {
                IsBusy = true;
                Appel = await _api.GetByIdAsync(Id);  // 👈 déclenche OnAppelChanged
            }
            finally
            {
                IsBusy = false;
            }
        }

        // 🔥 Modifier
        [RelayCommand]
        public async Task EditAsync()
        {
            if (!IsSyndic)
            {
                await Shell.Current.DisplayAlert("Accès restreint", "Seul le syndic peut modifier.", "OK");
                return;
            }

            await Shell.Current.GoToAsync($"appel-edit?id={Id}");
        }

        // 🔥 Clôturer
        [RelayCommand]
        public async Task CloturerAsync()
        {
            if (!IsSyndic || string.IsNullOrWhiteSpace(Id)) return;

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

        // 🔥 Supprimer
        [RelayCommand]
        public async Task DeleteAsync()
        {
            if (!IsSyndic || string.IsNullOrWhiteSpace(Id)) return;

            var ok = await Shell.Current.DisplayAlert("Suppression", "Supprimer cet appel ?", "Oui", "Non");
            if (!ok) return;

            await _api.DeleteAsync(Id);
            await Shell.Current.GoToAsync("//appels");
        }
    }
}
