using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using System.Net;

namespace SyndicApp.Mobile.ViewModels.Affectations
{
    [QueryProperty(nameof(IdParam), "id")]
    public partial class AffectationDetailsViewModel : ObservableObject
    {
        private readonly IAffectationsLotsApi _api;
        private readonly ILotsApi _lotsApi;

        public AffectationDetailsViewModel(IAffectationsLotsApi api, ILotsApi lotsApi)
        {
            _api = api;
            _lotsApi = lotsApi;

            CanDelete = true;
            CanEdit = true;
        }

        [ObservableProperty] private string? idParam;

        [ObservableProperty] private Guid id;

        [ObservableProperty] private AffectationLotDto? item;
        [ObservableProperty] private bool isBusy;

        [ObservableProperty] private bool canDelete;
        [ObservableProperty] private bool canEdit;

        [RelayCommand]
        public async Task LoadAsync()
        {
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

                if (Item != null)
                {
                    if (string.IsNullOrWhiteSpace(Item.LotNumero) && Item.LotId != Guid.Empty)
                    {
                        try
                        {
                            var lot = await _lotsApi.GetByIdAsync(Item.LotId);
                            if (lot != null)
                                Item.LotNumero = lot.NumeroLot;
                        }
                        catch
                        {
                        }
                    }

                    if (string.IsNullOrWhiteSpace(Item.UserNom) && Item.UserId != Guid.Empty)
                    {
                        try
                        {
                            var usersRes = await _api.GetAllUsersAsync(); 
                            var u = usersRes?.Data?.FirstOrDefault(x => x.Id == Item.UserId);
                            if (u != null)
                                Item.UserNom = !string.IsNullOrWhiteSpace(u.FullName)
                                    ? u.FullName!
                                    : (u.Email ?? u.Id.ToString());
                        }
                        catch
                        {
                        }
                    }

                    OnPropertyChanged(nameof(Item));
                }
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
        public async Task DeleteAsync()
        {
            if (!CanDelete)
            {
                await Shell.Current.DisplayAlert("Droits insuffisants",
                    "Tu n'as pas le droit de supprimer cette affectation.", "OK");
                return;
            }

            if (Item is null) return;

            var confirm = await Shell.Current.DisplayAlert(
                "Supprimer",
                "Confirmer la suppression de cette affectation ?",
                "Supprimer", "Annuler");
            if (!confirm) return;

            try
            {
                await _api.DeleteAsync(Item.Id);

                await Shell.Current.DisplayAlert("OK", "Affectation supprimée avec succès.", "OK");
                await Shell.Current.GoToAsync("//affectation-lots"); // ✅ retour vers la liste complète
            }
            catch (ApiException apiEx) when ((int)apiEx.StatusCode == 204)
            {
                await Shell.Current.DisplayAlert("OK", "Affectation supprimée avec succès.", "OK");
                await Shell.Current.GoToAsync("//affectation-lots");
            }
            catch (ApiException apiEx)
            {
                await Shell.Current.DisplayAlert("API",
                    $"{(int)apiEx.StatusCode} - {apiEx.StatusCode}\n{apiEx.Content}", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
        }

        [RelayCommand]
        public async Task CloturerAsync()
        {
            if (!CanEdit)
            {
                await Shell.Current.DisplayAlert("Droits insuffisants",
                    "Tu n'as pas le droit de clôturer cette affectation.", "OK");
                return;
            }

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
            if (!CanEdit)
            {
                await Shell.Current.DisplayAlert("Droits insuffisants",
                    "Tu n'as pas le droit de modifier le statut.", "OK");
                return;
            }

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
