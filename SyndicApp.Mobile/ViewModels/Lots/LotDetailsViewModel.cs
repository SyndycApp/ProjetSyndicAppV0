using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Common.Messages;
using SyndicApp.Mobile.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.ViewModels.Lots
{
    [QueryProperty(nameof(Id), "id")]
    public partial class LotDetailsViewModel : ObservableObject
    {
        private readonly ILotsApi _api;
        private readonly IAffectationsLotsApi _affectationsApi;

        [ObservableProperty] private string id = "";
        [ObservableProperty] private LotDto? lot;

        // 👇 NEW : occupation actuelle
        [ObservableProperty] private bool estOccupe;
        [ObservableProperty] private string statutLot = "Libre";
        [ObservableProperty] private string occupantDisplay = "Lot non occupé";

        // 👇 NEW : historique des affectations du lot
        public ObservableCollection<AffectationLotDto> Historique { get; } = new();

        public LotDetailsViewModel(ILotsApi api, IAffectationsLotsApi affectationsApi)
        {
            _api = api;
            _affectationsApi = affectationsApi;
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (!Guid.TryParse(Id, out var guid)) return;

            // 1) Charger le lot
            Lot = await _api.GetByIdAsync(guid);

            // 2) Charger l’occupant actuel
            AffectationLotDto? actif = null;
            try
            {
                actif = await _affectationsApi.GetOccupantActuelAsync(guid);
            }
            catch
            {
                // si 404 ou autre → on considère "Libre"
            }

            if (actif != null)
            {
                EstOccupe = true;
                StatutLot = "Occupé";

                var nom = !string.IsNullOrWhiteSpace(actif.NomComplet)
                    ? actif.NomComplet
                    : (!string.IsNullOrWhiteSpace(actif.UserNom) ? actif.UserNom : "Inconnu");

                OccupantDisplay = $"Occupant : {nom}";
            }
            else
            {
                EstOccupe = false;
                StatutLot = "Libre";
                OccupantDisplay = "Lot non occupé";
            }

            // 3) Charger l’historique
            Historique.Clear();
            try
            {
                var list = await _affectationsApi.GetHistoriqueByLotAsync(guid);

                foreach (var a in list.OrderByDescending(x => x.DateDebut))
                    Historique.Add(a);
            }
            catch
            {
                // si l’API n’est pas prête, on n’affiche rien
            }
        }

        [RelayCommand]
        public Task EditAsync()
            => string.IsNullOrWhiteSpace(Id)
                ? Task.CompletedTask
                : Shell.Current.GoToAsync($"lot-edit?id={Id}");

        [RelayCommand]
        public async Task DeleteAsync()
        {
            if (!Guid.TryParse(Id, out var guid)) return;

            if (!await Shell.Current.DisplayAlert("Suppression", "Supprimer ce lot ?", "Oui", "Non"))
                return;

            try
            {
                await _api.DeleteAsync(guid);
            }
            catch (ApiException ex)
            {
                await Shell.Current.DisplayAlert("Erreur API", ex.Content ?? ex.Message, "OK");
                return;
            }

            WeakReferenceMessenger.Default.Send(new LotChangedMessage(true));
            await Shell.Current.GoToAsync("//lots");
        }
    }
}
