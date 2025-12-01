using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Finances
{
    public partial class ChargesListViewModel : ObservableObject
    {
        private readonly IChargesApi _chargesApi;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private ObservableCollection<ChargeDto> charges = new();

        // 🔐 flag de rôle
        [ObservableProperty]
        private bool isSyndic;

        // 🔐 utilisé par le XAML (CanManageCharges) + par les commandes
        public bool CanManageCharges => IsSyndic;

        public ChargesListViewModel(IChargesApi chargesApi)
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
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                Charges.Clear();

                var items = await _chargesApi.GetAllAsync();
                if (items == null) return;

                foreach (var item in items)
                    Charges.Add(item);
            }
            finally
            {
                IsBusy = false;
            }
        }

        // 👉 bouton "+"
        [RelayCommand]
        private async Task NewChargeAsync()
        {
            if (!CanManageCharges)
            {
                await Shell.Current.DisplayAlert(
                    "Accès restreint",
                    "Seul le syndic peut créer une charge.",
                    "OK");
                return;
            }

            await Shell.Current.GoToAsync("charge-create");
        }

        // 👉 ouverture détails depuis la ligne
        [RelayCommand]
        private async Task OpenDetailsAsync(ChargeDto? charge)
        {
            if (charge == null) return;
            await Shell.Current.GoToAsync($"charge-details?id={charge.Id}");
        }

        [RelayCommand]
        private async Task EditAsync(ChargeDto? charge)
        {
            if (charge == null) return;

            if (!CanManageCharges)
            {
                await Shell.Current.DisplayAlert(
                    "Accès restreint",
                    "Vous n'avez pas les droits pour modifier cette charge.",
                    "OK");
                return;
            }

            await Shell.Current.GoToAsync($"charge-edit?id={charge.Id}");
        }

        [RelayCommand]
        private async Task DeleteAsync(ChargeDto? charge)
        {
            if (charge == null) return;

            if (!CanManageCharges)
            {
                await Shell.Current.DisplayAlert(
                    "Accès restreint",
                    "Vous n'avez pas les droits pour supprimer cette charge.",
                    "OK");
                return;
            }

            var confirm = await Shell.Current.DisplayAlert(
                "Confirmation",
                $"Supprimer la charge \"{charge.Nom}\" ?",
                "Oui", "Non");

            if (!confirm) return;

            await _chargesApi.DeleteAsync(charge.Id);
            Charges.Remove(charge);
        }
    }
}
