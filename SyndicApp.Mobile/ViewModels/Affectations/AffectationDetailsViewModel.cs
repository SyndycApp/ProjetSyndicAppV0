using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using System.Net;

namespace SyndicApp.Mobile.ViewModels.Affectations
{
    // ⇩⇩ reçoît "id" en string pour éviter le cast automatique foireux
    [QueryProperty(nameof(IdParam), "id")]
    public partial class AffectationDetailsViewModel : ObservableObject
    {
        private readonly IAffectationsLotsApi _api;

        public AffectationDetailsViewModel(IAffectationsLotsApi api)
        {
            _api = api;
        }

        // Route param brut
        [ObservableProperty] private string? idParam;

        // Guid interne (optionnel si tu veux l'avoir)
        [ObservableProperty] private Guid id;

        // Données affichées
        [ObservableProperty] private AffectationLotDto? item;
        [ObservableProperty] private bool isBusy;

        [RelayCommand]
        public async Task LoadAsync()
        {
            // Parse sûr
            if (!Guid.TryParse(IdParam, out var guid))
            {
                await Shell.Current.DisplayAlert("Navigation", "Identifiant invalide.", "OK");
                return;
            }
            Id = guid;

            try
            {
                IsBusy = true;
                Item = await _api.GetByIdAsync(guid);
            }
            catch (ApiException apiEx)
            {
                await Shell.Current.DisplayAlert("API", $"{apiEx.StatusCode}\n{apiEx.Content}", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task CloturerAsync()
        {
            if (Item is null) return;

            var input = await Shell.Current.DisplayPromptAsync(
                "Clôturer l’affectation",
                "Date de fin (jj/mm/aaaa)",
                accept: "Valider", cancel: "Annuler",
                initialValue: DateTime.Today.ToString("dd/MM/yyyy"),
                keyboard: Keyboard.Numeric
            );
            if (string.IsNullOrWhiteSpace(input)) return;

            if (!DateTime.TryParse(input, out var dateFin))
            {
                await Shell.Current.DisplayAlert("Format invalide", "Saisis une date valide.", "OK");
                return;
            }

            try
            {
                var dto = new AffectationClotureDto { DateFin = dateFin };
                var updated = await _api.CloturerAsync(Item.Id, dto); // OK si l'API renvoie 200 + body
                Item = updated;
                await Shell.Current.DisplayAlert("OK", "Affectation clôturée.", "OK");
            }
            catch (ApiException apiEx) when (apiEx.StatusCode == HttpStatusCode.NoContent)
            {
                // L’API a répondu 204: on considère succès et on recharge l’item
                Item = await _api.GetByIdAsync(Item.Id);
                await Shell.Current.DisplayAlert("OK", "Affectation clôturée.", "OK");
            }
            catch (ApiException apiEx)
            {
                await Shell.Current.DisplayAlert("API", $"{(int)apiEx.StatusCode} - {apiEx.StatusCode}\n{apiEx.Content}", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
        }

        [RelayCommand]
        public async Task ToggleStatutAsync()
        {
            if (Item is null) return;

            var nouveau = !Item.EstProprietaire;
            var ok = await Shell.Current.DisplayAlert(
                "Changer le statut",
                $"Passer « Est propriétaire » à {(nouveau ? "Oui" : "Non")} ?",
                "Oui", "Non");
            if (!ok) return;

            try
            {
                var dto = new AffectationChangerStatutDto { EstProprietaire = nouveau };
                var updated = await _api.ChangerStatutAsync(Item.Id, dto); // OK si 200 + body
                Item = updated;
                await Shell.Current.DisplayAlert("OK", "Statut modifié.", "OK");
            }
            catch (ApiException apiEx) when (apiEx.StatusCode == HttpStatusCode.NoContent)
            {
                // 204: succès silencieux → on recharge
                Item = await _api.GetByIdAsync(Item.Id);
                await Shell.Current.DisplayAlert("OK", "Statut modifié.", "OK");
            }
            catch (ApiException apiEx)
            {
                await Shell.Current.DisplayAlert("API", $"{(int)apiEx.StatusCode} - {apiEx.StatusCode}\n{apiEx.Content}", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
        }

        [RelayCommand]
        public Task OpenHistoriqueAsync()
        {
            if (Item is null) return Task.CompletedTask;
            return Shell.Current.GoToAsync($"affectation-historique?lotId={Item.LotId}");
        }
    }
}
