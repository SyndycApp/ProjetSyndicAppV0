using System;
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
    public partial class ChargeDetailsViewModel : ObservableObject
    {
        private readonly IChargesApi _chargesApi;

        [ObservableProperty]
        private string id = string.Empty;

        [ObservableProperty]
        private ChargeDto? charge;

        [ObservableProperty]
        private bool isBusy;

        // 🔐 rôle
        [ObservableProperty]
        private bool isSyndic;

        public bool CanManageCharges => IsSyndic;

        public ChargeDetailsViewModel(IChargesApi chargesApi)
        {
            _chargesApi = chargesApi;

            var role = Preferences.Get("user_role", string.Empty)
                                  ?.ToLowerInvariant()
                                  ?? string.Empty;

            IsSyndic = role.Contains("syndic");
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy || string.IsNullOrWhiteSpace(Id)) return;

            try
            {
                IsBusy = true;
                var guid = Guid.Parse(Id);
                Charge = await _chargesApi.GetByIdAsync(guid);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task EditAsync()
        {
            if (string.IsNullOrWhiteSpace(Id)) return;

            if (!CanManageCharges)
            {
                await Shell.Current.DisplayAlert(
                    "Accès restreint",
                    "Vous n'avez pas les droits pour modifier cette charge.",
                    "OK");
                return;
            }

            await Shell.Current.GoToAsync($"charge-edit?id={Id}");
        }

        [RelayCommand]
        public async Task DeleteAsync()
        {
            if (string.IsNullOrWhiteSpace(Id)) return;

            if (!CanManageCharges)
            {
                await Shell.Current.DisplayAlert(
                    "Accès restreint",
                    "Vous n'avez pas les droits pour supprimer cette charge.",
                    "OK");
                return;
            }

            var confirm = await Shell.Current.DisplayAlert(
                "Suppression",
                "Supprimer cette charge ?",
                "Oui", "Non");

            if (!confirm) return;

            var guid = Guid.Parse(Id);
            await _chargesApi.DeleteAsync(guid);
            await Shell.Current.GoToAsync("//charges");
        }
    }
}
