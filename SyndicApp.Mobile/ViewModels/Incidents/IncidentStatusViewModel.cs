using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
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

        // ---- Props ----
        [ObservableProperty] private string? incidentId;

        [ObservableProperty] private List<string> statuts;
        [ObservableProperty] private string selectedStatut;

        [ObservableProperty] private string? commentaire;

        // Nom lisible qui s'affiche dans la page
        [ObservableProperty] private string? auteurNom;

        // ---- Chargement initial de la page ----
        [RelayCommand]
        public async Task LoadAsync()
        {
            // 0) Vérif de l'id incident
            if (string.IsNullOrWhiteSpace(IncidentId) ||
                !Guid.TryParse(IncidentId, out var guid))
                return;

            // 1) Charger l’incident pour pré-remplir le statut
            var incident = await _incidentsApi.GetByIdAsync(guid);
            if (!string.IsNullOrWhiteSpace(incident.Statut) &&
                Statuts.Contains(incident.Statut))
            {
                SelectedStatut = incident.Statut;
            }

            // 2) Récupérer l'utilisateur connecté (comme dans LoginViewModel)
            try
            {
                var me = await _accountApi.MeAsync();

                // On AFFICHE un nom lisible
                if (!string.IsNullOrWhiteSpace(me.FullName))
                {
                    AuteurNom = me.FullName;
                }
                else if (!string.IsNullOrWhiteSpace(me.Email))
                {
                    AuteurNom = me.Email!;
                }
                else
                {
                    // Dernier fallback: on montre l'Id (au moins tu vois que /me marche)
                    AuteurNom = me.Id.ToString();
                }
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Token non envoyé / expiré / utilisateur non authentifié
                AuteurNom = "Non authentifié";
                await Shell.Current.DisplayAlert(
                    "Session expirée",
                    "Impossible de récupérer les infos de l’utilisateur connecté. " +
                    "Connecte-toi à nouveau.",
                    "OK");
            }
            catch (Exception)
            {
                // En cas d'autre erreur API, on évite de crasher
                AuteurNom = "Utilisateur inconnu";
            }
        }

        // ---- Enregistrement ----
        [RelayCommand]
        public async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(IncidentId) ||
                !Guid.TryParse(IncidentId, out var guid))
                return;

            // On récupère l'utilisateur connecté pour envoyer son Id à l’API
            UserDto me;
            try
            {
                me = await _accountApi.MeAsync();
            }
            catch
            {
                await Shell.Current.DisplayAlert(
                    "Erreur",
                    "Impossible de récupérer l’utilisateur connecté. Réessaie après t’être reconnecté.",
                    "OK");
                return;
            }

            var body = new IncidentStatusUpdateRequest
            {
                Statut = SelectedStatut,
                AuteurId = me.Id,                  // => Guid correct envoyé à l’API
                Commentaire = Commentaire ?? string.Empty
            };

            // IIncidentsApi.UpdateStatusAsync attend un string id → on passe IncidentId tel quel
            await _incidentsApi.UpdateStatusAsync(IncidentId, body);

            await Shell.Current.DisplayAlert("Succès", "Statut mis à jour.", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
