using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Finances
{
    [QueryProperty(nameof(Id), "id")]
    public partial class AppelEditViewModel : ObservableObject
    {
        private readonly IAppelsApi _api;
        private readonly IResidencesApi _residencesApi;

        [ObservableProperty] private string id = string.Empty;
        [ObservableProperty] private string? description;
        [ObservableProperty] private decimal montantTotal;
        [ObservableProperty] private DateTime dateEmission = DateTime.Today;
        [ObservableProperty] private string residenceId = string.Empty;

        [ObservableProperty] private int nbPaiements;
        [ObservableProperty] private decimal montantPaye;
        [ObservableProperty] private decimal montantReste;

        [ObservableProperty] private bool isBusy;

        [ObservableProperty] private List<ResidenceDto> residences = new();
        [ObservableProperty] private ResidenceDto? selectedResidence;

        public AppelEditViewModel(IAppelsApi api, IResidencesApi residencesApi)
        {
            _api = api;
            _residencesApi = residencesApi;
            _ = LoadResidencesAsync();
        }

        [RelayCommand]
        public async Task LoadResidencesAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                Residences = await _residencesApi.GetAllAsync() ?? new List<ResidenceDto>();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(
                    "Erreur",
                    "Impossible de charger les résidences : " + ex.Message,
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy || string.IsNullOrWhiteSpace(Id)) return;

            try
            {
                IsBusy = true;

                var dto = await _api.GetByIdAsync(Id);
                Description = dto.Description;
                MontantTotal = dto.MontantTotal;
                DateEmission = dto.DateEmission == default ? DateTime.Today : dto.DateEmission;
                ResidenceId = dto.ResidenceId;
                NbPaiements = dto.NbPaiements;
                MontantPaye = dto.MontantPaye;
                MontantReste = dto.MontantReste;

                if (Residences == null || Residences.Count == 0)
                {
                    Residences = await _residencesApi.GetAllAsync() ?? new List<ResidenceDto>();
                }

                SelectedResidence = Residences.FirstOrDefault(r => r.Id.ToString() == ResidenceId);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
            if (IsBusy || string.IsNullOrWhiteSpace(Id)) return;

            try
            {
                IsBusy = true;

                if (SelectedResidence != null)
                    ResidenceId = SelectedResidence.Id.ToString();

                var payload = new AppelDeFondsDto
                {
                    Id = Id,
                    Description = Description,
                    MontantTotal = MontantTotal,
                    DateEmission = DateEmission == default ? DateTime.Today : DateEmission,
                    ResidenceId = ResidenceId,
                    NbPaiements = NbPaiements,
                    MontantPaye = MontantPaye,
                    MontantReste = MontantReste
                };

                await _api.UpdateAsync(Id, payload);

                await Shell.Current.DisplayAlert(
                    "Succès",
                    "Appel modifié avec succès.",
                    "OK");

                await Shell.Current.GoToAsync($"appel-details?id={Id}");
            }
            catch (ValidationApiException ex)
            {
                var raw = ex.Content?.ToString();
                var message = string.IsNullOrWhiteSpace(raw) ? ex.Message : raw;

                await Shell.Current.DisplayAlert(
                    "Erreur de validation",
                    message,
                    "OK");
            }
            catch (ApiException ex)
            {
                await Shell.Current.DisplayAlert(
                    "Erreur API",
                    $"Code : {(int)ex.StatusCode}\n{ex.Content}",
                    "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(
                    "Erreur",
                    ex.Message,
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task CloturerAsync()
        {
            if (string.IsNullOrWhiteSpace(Id)) return;

            await _api.CloturerAsync(Id);
            await Shell.Current.GoToAsync($"appel-details?id={Id}");
        }

        [RelayCommand]
        public async Task DeleteAsync()
        {
            if (string.IsNullOrWhiteSpace(Id)) return;

            var ok = await Shell.Current.DisplayAlert(
                "Suppression",
                "Supprimer cet appel ?",
                "Oui",
                "Non");

            if (!ok) return;

            try
            {
                await _api.DeleteAsync(Id);
                await Shell.Current.DisplayAlert("Succès", "Appel supprimé.", "OK");
                await Shell.Current.GoToAsync("//appels");
            }
            catch (ApiException ex)
            {
                await Shell.Current.DisplayAlert("Erreur API", ex.Content ?? ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
        }
    }
}
