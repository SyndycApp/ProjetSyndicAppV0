using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Android.Util.EventLogTags;

namespace SyndicApp.Mobile.ViewModels.Incidents
{
    public partial class IncidentCreateViewModel : ObservableObject
    {
        private readonly IIncidentsApi _incidentsApi;
        private readonly IResidencesApi _residencesApi;
        private readonly ILotsApi _lotsApi;
        private readonly IAuthApi _authApi;

        public IncidentCreateViewModel(
            IIncidentsApi incidentsApi,
            IResidencesApi residencesApi,
            ILotsApi lotsApi,
            IAuthApi authApi)
        {
            _incidentsApi = incidentsApi;
            _residencesApi = residencesApi;
            _lotsApi = lotsApi;
            _authApi = authApi;

            Residences = new();
            Lots = new();
            Users = new();
            Urgences = new() { "Basse", "Normale", "Elevee", "Critique" };
            Statuts = new() { "Ouvert", "EnCours", "Resolue", "Cloture" };

            SelectedUrgence = "Normale";
            SelectedStatut = "Ouvert";
        }

        // Pickers
        [ObservableProperty] private List<ResidenceDto> residences;
        [ObservableProperty] private ResidenceDto? selectedResidence;

        [ObservableProperty] private List<LotDto> lots;
        [ObservableProperty] private LotDto? selectedLot;

        [ObservableProperty] private List<UserDto> users;
        [ObservableProperty] private UserDto? selectedUser;

        [ObservableProperty] private List<string> urgences;
        [ObservableProperty] private string selectedUrgence;

        [ObservableProperty] private List<string> statuts;
        [ObservableProperty] private string selectedStatut;

        // Champs texte
        [ObservableProperty] private string? titre;
        [ObservableProperty] private string? description;
        [ObservableProperty] private string? typeIncident;

        [RelayCommand]
        public async Task LoadAsync()
        {
            var residencesTask = _residencesApi.GetAllAsync();
            var lotsTask = _lotsApi.GetAllAsync();
            var usersTask = _authApi.GetAllAsync();

            await Task.WhenAll(residencesTask, lotsTask, usersTask);

            Residences = residencesTask.Result?
                .OrderBy(r => r.Nom)
                .ToList() ?? new List<ResidenceDto>();

            Lots = lotsTask.Result?
                .OrderBy(l => l.NumeroLot)
                .ToList() ?? new List<LotDto>();

            var usersResp = usersTask.Result;
            if (usersResp?.Success == true && usersResp.Data != null)
            {
                Users = usersResp.Data
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        Email = u.Email ?? string.Empty,
                        FullName = !string.IsNullOrWhiteSpace(u.FullName)
                            ? u.FullName!
                            : (u.Email ?? u.Id.ToString()),
                        Roles = u.Roles ?? new List<string>()
                    })
                    .OrderBy(u => u.FullName)
                    .ToList();
            }
            else
            {
                Users = new List<UserDto>();
            }
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Titre) ||
                string.IsNullOrWhiteSpace(Description) ||
                string.IsNullOrWhiteSpace(TypeIncident) ||
                SelectedResidence == null ||
                SelectedLot == null ||
                SelectedUser == null)
            {
                await Shell.Current.DisplayAlert(
                    "Erreur",
                    "Merci de remplir tous les champs et de sélectionner résidence, lot et utilisateur.",
                    "OK");
                return;
            }

            var request = new IncidentCreateRequest
            {
                Titre = Titre,
                Description = Description,
                TypeIncident = TypeIncident,
                Urgence = SelectedUrgence,
                ResidenceId = SelectedResidence.Id,
                LotId = SelectedLot.Id,
                DeclareParId = SelectedUser.Id
            };

            await _incidentsApi.CreateAsync(request);

            await Shell.Current.DisplayAlert("Succès", "Incident créé.", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
