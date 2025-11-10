using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Common.Messages;
using SyndicApp.Mobile.Models;
using System;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.ViewModels.Lots
{
    [QueryProperty(nameof(Id), "id")]
    public partial class LotDetailsViewModel : ObservableObject
    {
        private readonly ILotsApi _api;

        [ObservableProperty] string id = "";
        [ObservableProperty] LotDto? lot;

        public LotDetailsViewModel(ILotsApi api) => _api = api;

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (!Guid.TryParse(Id, out var guid)) return;
            Lot = await _api.GetByIdAsync(guid);
        }

        [RelayCommand]
        public Task EditAsync()
            => string.IsNullOrWhiteSpace(Id) ? Task.CompletedTask : Shell.Current.GoToAsync($"lot-edit?id={Id}");

        [RelayCommand]
        public async Task DeleteAsync()
        {
            if (!Guid.TryParse(Id, out var guid)) return;
            if (!await Shell.Current.DisplayAlert("Suppression", "Supprimer ce lot ?", "Oui", "Non")) return;
            try { await _api.DeleteAsync(guid); }
            catch (ApiException ex) { await Shell.Current.DisplayAlert("Erreur API", ex.Content ?? ex.Message, "OK"); return; }

            WeakReferenceMessenger.Default.Send(new LotChangedMessage(true));
            await Shell.Current.GoToAsync("//lots");
        }
    }
}
