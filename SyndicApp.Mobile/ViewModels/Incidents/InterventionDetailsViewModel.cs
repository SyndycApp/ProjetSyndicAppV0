// SyndicApp.Mobile/ViewModels/Incidents/InterventionDetailsViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntelliJ.Lang.Annotations;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using System;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.ViewModels.Incidents
{
    [QueryProperty(nameof(IdParam), "id")]
    public partial class InterventionDetailsViewModel : ObservableObject
    {
        private readonly IInterventionsApi _api;
        private readonly IAccountApi _accountApi; // pour récupérer l'utilisateur courant (AuteurId)

        public InterventionDetailsViewModel(IInterventionsApi api, IAccountApi accountApi)
        {
            _api = api;
            _accountApi = accountApi;
        }

        [ObservableProperty] private string? idParam;
        [ObservableProperty] private Guid id;
        [ObservableProperty] private InterventionDto? item;
        [ObservableProperty] private bool isBusy;

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (!Guid.TryParse(IdParam, out var guid))
            {
                await Shell.Current.DisplayAlert("Erreur", "Identifiant invalide.", "OK");
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
                await Shell.Current.DisplayAlert("API", apiEx.Content ?? apiEx.Message, "OK");
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
            if (Item is null) return;

            var ok = await Shell.Current.DisplayAlert(
                "Supprimer",
                "Confirmer la suppression de cette intervention ?",
                "Supprimer", "Annuler");

            if (!ok) return;

            try
            {
                await _api.DeleteAsync(Item.Id);
                await Shell.Current.DisplayAlert("OK", "Intervention supprimée.", "OK");
                await Shell.Current.GoToAsync("//interventions");
            }
            catch (ApiException apiEx)
            {
                await Shell.Current.DisplayAlert("API", apiEx.Content ?? apiEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
        }

        private async Task<Guid> GetCurrentUserIdAsync()
        {
            try
            {
                var me = await _accountApi.MeAsync();
                return me.Id;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        private async Task ChangeStatusAsync(StatutIntervention newStatus)
        {
            if (Item is null) return;

            var auteurId = await GetCurrentUserIdAsync();
            DateTime? dateRealisation = null;
            decimal? cout = null;
            string? commentaire = null;

            if (newStatus == StatutIntervention.Terminee)
            {
                var dateStr = await Shell.Current.DisplayPromptAsync(
                    "Terminer l'intervention",
                    "Date de réalisation (jj/mm/aaaa)",
                    "Valider", "Annuler",
                    initialValue: DateTime.Today.ToString("dd/MM/yyyy"));

                if (string.IsNullOrWhiteSpace(dateStr)) return;

                if (!DateTime.TryParse(dateStr, out var d))
                {
                    await Shell.Current.DisplayAlert("Format invalide", "Date invalide.", "OK");
                    return;
                }

                dateRealisation = d;

                var coutStr = await Shell.Current.DisplayPromptAsync(
                    "Coût réel",
                    "Montant (optionnel)",
                    "Valider", "Ignorer",
                    keyboard: Keyboard.Numeric);

                if (!string.IsNullOrWhiteSpace(coutStr) && decimal.TryParse(coutStr, out var c))
                    cout = c;
            }

            commentaire = await Shell.Current.DisplayPromptAsync(
                "Commentaire",
                "Commentaire (optionnel)",
                "OK", "Ignorer");

            try
            {
                IsBusy = true;

                var dto = new InterventionChangeStatusRequest
                {
                    Statut = newStatus,
                    DateRealisation = dateRealisation,
                    CoutReel = cout,
                    AuteurId = auteurId,
                    Commentaire = commentaire
                };

                Item = await _api.ChangeStatusAsync(Item.Id, dto);
                await Shell.Current.DisplayAlert("OK", "Statut mis à jour.", "OK");
            }
            catch (ApiException apiEx)
            {
                await Shell.Current.DisplayAlert("API", apiEx.Content ?? apiEx.Message, "OK");
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
        public Task StartAsync() => ChangeStatusAsync(StatutIntervention.EnCours);

        [RelayCommand]
        public Task CompleteAsync() => ChangeStatusAsync(StatutIntervention.Terminee);

        [RelayCommand]
        public Task CancelAsync() => ChangeStatusAsync(StatutIntervention.Annulee);
    }
}
