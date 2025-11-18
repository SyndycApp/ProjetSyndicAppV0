using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Incidents
{
    public partial class DevisTravauxCreateViewModel : ObservableObject
    {
        private readonly IDevisTravauxApi _devisApi;
        private readonly IResidencesApi _residencesApi;
        private readonly IIncidentsApi _incidentsApi;

        [ObservableProperty] private bool isBusy;

        [ObservableProperty] private string titre = string.Empty;
        [ObservableProperty] private string description = string.Empty;
        [ObservableProperty] private decimal montantHT;
        [ObservableProperty] private decimal tauxTVA;

        public ObservableCollection<ResidenceDto> Residences { get; } = new();
        public ObservableCollection<IncidentDto> Incidents { get; } = new();

        [ObservableProperty] private ResidenceDto? selectedResidence;
        [ObservableProperty] private IncidentDto? selectedIncident;

        public DevisTravauxCreateViewModel(
            IDevisTravauxApi devisApi,
            IResidencesApi residencesApi,
            IIncidentsApi incidentsApi)
        {
            _devisApi = devisApi;
            _residencesApi = residencesApi;
            _incidentsApi = incidentsApi;
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                Residences.Clear();
                Incidents.Clear();

                var resList = await _residencesApi.GetAllAsync();
                foreach (var r in resList)
                    Residences.Add(r);

                var incList = await _incidentsApi.GetAllAsync();
                foreach (var i in incList)
                    Incidents.Add(i);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(
                    "Erreur",
                    $"Chargement des données : {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
            if (IsBusy) return;

            // Debug pour vérifier que le bouton marche
            await Shell.Current.DisplayAlert("Debug", "SaveAsync exécuté", "OK");

            try
            {
                if (SelectedResidence == null)
                {
                    await Shell.Current.DisplayAlert(
                        "Validation",
                        "La résidence est obligatoire.",
                        "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Titre))
                {
                    await Shell.Current.DisplayAlert(
                        "Validation",
                        "Le titre est obligatoire.",
                        "OK");
                    return;
                }

                if (MontantHT <= 0)
                {
                    await Shell.Current.DisplayAlert(
                        "Validation",
                        "Le montant HT doit être supérieur à 0.",
                        "OK");
                    return;
                }

                IsBusy = true;

                var req = new DevisTravauxCreateRequest
                {
                    Titre = Titre.Trim(),
                    Description = Description?.Trim() ?? string.Empty,
                    MontantHT = MontantHT,
                    TauxTVA = TauxTVA,
                    ResidenceId = SelectedResidence.Id,
                    IncidentId = SelectedIncident != null
                        ? SelectedIncident.Id
                        : Guid.Empty
                };

                var created = await _devisApi.CreateAsync(req);

                await Shell.Current.DisplayAlert(
                    "Succès",
                    "Devis créé avec succès.",
                    "OK");

                await Shell.Current.GoToAsync("//devis-travaux");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(
                    "Erreur",
                    $"Erreur lors de la création : {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public Task CancelAsync()
            => Shell.Current.GoToAsync("//devis-travaux");
    }
}
