using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Incidents
{
    [QueryProperty(nameof(IncidentId), "id")]
    public partial class IncidentStatusViewModel : ObservableObject
    {
        private readonly IIncidentsApi _incidentsApi;
        private readonly IAccountApi _accountApi;

        public IncidentStatusViewModel(IIncidentsApi incidentsApi, IAccountApi accountApi)
        {
            _incidentsApi = incidentsApi;
            _accountApi = accountApi;

            Statuts = new() { "Ouvert", "EnCours", "Resolue", "Cloture" };
            SelectedStatut = "EnCours";
        }

        [ObservableProperty] private string? incidentId;

        [ObservableProperty] private List<string> statuts;

        // non-nullable, on l'initialise dans le constructeur
        [ObservableProperty] private string selectedStatut;

        [ObservableProperty] private string? commentaire;

        // Appelé depuis la page (OnAppearing)
        public async Task LoadAsync()
        {
            // Ici on peut juste vérifier que l'utilisateur est bien récupérable.
            // On ne stocke rien pour ne pas changer ton comportement existant.
            try
            {
                _ = await _accountApi.MeAsync();
            }
            catch
            {
                // pas bloquant, on gérera dans SaveAsync
            }
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(IncidentId))
                return;

            // Récupération de l'utilisateur connecté (comme pour paiements)
            var me = await _accountApi.MeAsync();
            var auteurId = me.Id;

            var body = new IncidentStatusUpdateRequest
            {
                Statut = SelectedStatut,
                AuteurId = auteurId,
                Commentaire = Commentaire ?? string.Empty
            };

            // ⚠️ ton interface Refit attend un string pour {id}, pas Guid
            await _incidentsApi.UpdateStatusAsync(IncidentId, body);

            await Shell.Current.DisplayAlert("Succès", "Statut mis à jour.", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
