using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Services;

namespace SyndicApp.Mobile.ViewModels.Incidents
{
    [QueryProperty(nameof(DevisId), "id")]
    public partial class DevisTravauxDetailsViewModel : ObservableObject
    {
        private readonly IDevisTravauxApi _devisApi;
        private readonly IResidencesApi _residencesApi;
        private readonly IIncidentsApi _incidentsApi;
        private readonly IAuthApi _authApi;
        private readonly IAccountApi _accountApi;

        [ObservableProperty] private string? devisId;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private bool isSyndic;

        // Devis
        [ObservableProperty] private string? titre;
        [ObservableProperty] private string? description;
        [ObservableProperty] private decimal montantHT;
        [ObservableProperty] private decimal tauxTVA;
        [ObservableProperty] private decimal montantTTC;
        [ObservableProperty] private string? statut;
        [ObservableProperty] private DateTime dateEmission;

        // Résidence
        [ObservableProperty] private string? residenceNom;
        [ObservableProperty] private string? residenceAdresseComplete;

        // Incident
        [ObservableProperty] private string? incidentTitre;
        [ObservableProperty] private string? incidentDescription;
        [ObservableProperty] private string? incidentType;
        [ObservableProperty] private string? incidentUrgence;
        [ObservableProperty] private string? incidentStatut;
        [ObservableProperty] private DateTime incidentDateDeclaration;
        [ObservableProperty] private string? incidentDeclarantNom;

        // Décision
        [ObservableProperty] private string? commentaireDecision;
        [ObservableProperty] private DateTime? dateDecision;
        [ObservableProperty] private string? statutDecision;

        public DevisTravauxDetailsViewModel(
            IDevisTravauxApi devisApi,
            IResidencesApi residencesApi,
            IIncidentsApi incidentsApi,
            IAuthApi authApi,
            IAccountApi accountApi)
        {
            _devisApi = devisApi;
            _residencesApi = residencesApi;
            _incidentsApi = incidentsApi;
            _authApi = authApi;
            _accountApi = accountApi;
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy) return;
            if (string.IsNullOrWhiteSpace(DevisId) || !Guid.TryParse(DevisId, out var guid))
                return;

            IsBusy = true;
            try
            {
                // Rôle utilisateur
                var me = await _accountApi.MeAsync();
                IsSyndic = me.Roles?.FirstOrDefault() == "Syndic";

                var devis = await _devisApi.GetByIdAsync(guid);

                Titre = devis.Titre;
                Description = devis.Description;
                MontantHT = devis.MontantHT;
                TauxTVA = devis.TauxTVA;
                MontantTTC = devis.MontantTTC;
                Statut = devis.Statut;
                DateEmission = devis.DateEmission;

                StatutDecision = devis.Statut;
                DateDecision = devis.DateDecision;
                CommentaireDecision = devis.CommentaireDecision;

                // Résidence
                var residence = await _residencesApi.GetByIdAsync(devis.ResidenceId.ToString());
                ResidenceNom = residence.Nom;
                ResidenceAdresseComplete = $"{residence.Adresse}, {residence.Ville} {residence.CodePostal}";

                // Incident lié
                if (devis.IncidentId.HasValue)
                {
                    var incident = await _incidentsApi.GetByIdAsync(devis.IncidentId.Value);

                    IncidentTitre = incident.Titre;
                    IncidentDescription = incident.Description;
                    IncidentType = incident.TypeIncident;
                    IncidentUrgence = incident.Urgence;
                    IncidentStatut = incident.Statut;
                    IncidentDateDeclaration = incident.DateDeclaration;

                    // Nom déclarant via AuthApi.GetAllAsync
                    var usersResp = await _authApi.GetAllAsync();
                    var user = usersResp?.Data?.FirstOrDefault(u => u.Id == incident.DeclareParId);

                    IncidentDeclarantNom = user?.FullName ?? user?.Email ?? incident.DeclareParId.ToString();
                }
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
        public Task GoBackAsync() => Shell.Current.GoToAsync("..");

        [RelayCommand]
        public Task GoToDecisionAsync()
            => Shell.Current.GoToAsync($"devis-decision?id={DevisId}");

        [RelayCommand]
        public async Task DeleteAsync()
        {
            if (!Guid.TryParse(DevisId, out var guid)) return;

            var confirm = await Shell.Current.DisplayAlert("Confirmation", "Supprimer ce devis ?", "Oui", "Non");
            if (!confirm) return;

            try
            {
                await _devisApi.DeleteAsync(guid);
                await Shell.Current.DisplayAlert("Info", "Devis supprimé.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
        }
    }
}
