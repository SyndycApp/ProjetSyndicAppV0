using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.ViewModels.Incidents
{
    [QueryProperty(nameof(IncidentId), "id")]
    public partial class IncidentDetailsViewModel : ObservableObject
    {
        private readonly IIncidentsApi _incidentsApi;
        private readonly ILotsApi _lotsApi;
        private readonly IResidencesApi _residencesApi;
        private readonly IAuthApi _authApi;

        public IncidentDetailsViewModel(
            IIncidentsApi incidentsApi,
            ILotsApi lotsApi,
            IResidencesApi residencesApi,
            IAuthApi authApi)
        {
            _incidentsApi = incidentsApi;
            _lotsApi = lotsApi;
            _residencesApi = residencesApi;
            _authApi = authApi;
        }

        [ObservableProperty] private string? incidentId;

        // Infos incident
        [ObservableProperty] private string? titre;
        [ObservableProperty] private string? description;
        [ObservableProperty] private string? typeIncident;
        [ObservableProperty] private string? urgence;
        [ObservableProperty] private string? statut;
        [ObservableProperty] private DateTime dateDeclaration;

        // Déclarant
        [ObservableProperty] private string? declarantNom;

        // Résidence / Lot
        [ObservableProperty] private string? residenceNom;
        [ObservableProperty] private string? residenceAdresseComplete;
        [ObservableProperty] private string? lotNumero;

        // Historique
        [ObservableProperty]
        private ObservableCollection<IncidentHistoriqueItem> historique
            = new ObservableCollection<IncidentHistoriqueItem>();

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (string.IsNullOrWhiteSpace(IncidentId) ||
                !Guid.TryParse(IncidentId, out var guid))
                return;

            var inc = await _incidentsApi.GetByIdAsync(guid);


            Titre = inc.Titre;
            Description = inc.Description;
            TypeIncident = inc.TypeIncident;
            Urgence = inc.Urgence;
            Statut = inc.Statut;
            DateDeclaration = inc.DateDeclaration;


            var usersResp = await _authApi.GetAllAsync();
            var user = usersResp?.Data?.FirstOrDefault(u => u.Id == inc.DeclareParId);

            DeclarantNom = user != null
                ? (!string.IsNullOrWhiteSpace(user.FullName)
                    ? user.FullName
                    : (user.Email ?? user.Id.ToString()))
                : inc.DeclareParId?.ToString();


            if (inc.LotId != null)
            {
                var lot = await _lotsApi.GetByIdAsync(inc.LotId.Value);
                LotNumero = lot?.NumeroLot ?? "—";
            }
            else
            {
                LotNumero = "—";
            }

            if (inc.ResidenceId != null)
            {
                var residence = await _residencesApi.GetByIdAsync(inc.ResidenceId.Value.ToString());
                ResidenceNom = residence?.Nom;
                ResidenceAdresseComplete =
                    residence != null
                        ? $"{residence.Adresse}, {residence.Ville} {residence.CodePostal}"
                        : "—";
            }
            else
            {
                ResidenceNom = "—";
                ResidenceAdresseComplete = "—";
            }

            Historique.Clear();
            if (inc.Historique != null)
            {
                foreach (var h in inc.Historique.OrderByDescending(x => x.DateAction))
                {
                    Historique.Add(h);
                }
            }
        }

        [RelayCommand]
        public Task GoBack()
            => Shell.Current.GoToAsync("..");

        [RelayCommand]
        public Task GoToEdit()
            => Shell.Current.GoToAsync($"incident-edit?id={IncidentId}");

        [RelayCommand]
        public Task GoToChangeStatus()
            => Shell.Current.GoToAsync($"incident-status?id={IncidentId}");

        [RelayCommand]
        public async Task DeleteAsync()
        {
            if (string.IsNullOrWhiteSpace(IncidentId) ||
                !Guid.TryParse(IncidentId, out var guid))
                return;

            var confirm = await Shell.Current.DisplayAlert(
                "Confirmation",
                "Êtes-vous sûr de vouloir supprimer cet incident ?",
                "Oui", "Non");

            if (!confirm) return;

            await _incidentsApi.DeleteAsync(guid);
            await Shell.Current.DisplayAlert("Info", "Incident supprimé.", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
