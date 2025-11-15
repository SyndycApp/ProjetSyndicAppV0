using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntelliJ.Lang.Annotations;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using System;
using System.Threading.Tasks;

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

        public ChargeDetailsViewModel(IChargesApi chargesApi)
        {
            _chargesApi = chargesApi;
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
            await Shell.Current.GoToAsync($"charge-edit?id={Id}");
        }

        [RelayCommand]
        public async Task DeleteAsync()
        {
            if (string.IsNullOrWhiteSpace(Id)) return;

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
