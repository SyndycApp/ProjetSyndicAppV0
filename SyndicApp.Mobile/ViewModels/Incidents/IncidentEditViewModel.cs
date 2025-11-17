using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.ViewModels.Incidents
{
    [QueryProperty(nameof(IncidentId), "id")]
    public partial class IncidentEditViewModel : ObservableObject
    {
        private readonly IIncidentsApi _incidentsApi;
        private readonly ILotsApi _lotsApi;

        public IncidentEditViewModel(IIncidentsApi incidentsApi, ILotsApi lotsApi)
        {
            _incidentsApi = incidentsApi;
            _lotsApi = lotsApi;

            Lots = new();
            Urgences = new() { "Basse", "Normale", "Elevee", "Critique" };
        }

        [ObservableProperty] private string? incidentId;

        [ObservableProperty] private string? description;
        [ObservableProperty] private string? typeIncident;
        [ObservableProperty] private string? selectedUrgence;

        [ObservableProperty] private List<LotDto> lots;
        [ObservableProperty] private LotDto? selectedLot;

        // manquante avant → ajoutée
        [ObservableProperty] private List<string> urgences;

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (string.IsNullOrWhiteSpace(IncidentId) ||
                !Guid.TryParse(IncidentId, out var guid))
                return;

            // Charger les lots pour le picker
            Lots = (await _lotsApi.GetAllAsync())?
                .OrderBy(l => l.NumeroLot)
                .ToList() ?? new List<LotDto>();

            // Charger l'incident
            var inc = await _incidentsApi.GetByIdAsync(guid);

            Description = inc.Description;
            TypeIncident = inc.TypeIncident;
            SelectedUrgence = inc.Urgence;

            SelectedLot = Lots.FirstOrDefault(l => l.Id == inc.LotId);
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(IncidentId))
                return;

            if (SelectedLot == null ||
                string.IsNullOrWhiteSpace(Description) ||
                string.IsNullOrWhiteSpace(TypeIncident) ||
                string.IsNullOrWhiteSpace(SelectedUrgence))
            {
                await Shell.Current.DisplayAlert("Erreur", "Merci de remplir tous les champs.", "OK");
                return;
            }

            var body = new IncidentUpdateRequest
            {
                Titre = "", // le backend ne l’utilise pas dans ton exemple PUT
                Description = Description,
                TypeIncident = TypeIncident,
                Urgence = SelectedUrgence,
                LotId = SelectedLot.Id
            };

            // ⚠️ l’interface Refit attend un string pour {id}
            await _incidentsApi.UpdateAsync(IncidentId, body);

            await Shell.Current.DisplayAlert("Succès", "Incident mis à jour.", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
