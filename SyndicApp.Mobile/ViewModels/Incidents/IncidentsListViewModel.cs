using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Incidents
{
    public partial class IncidentsListViewModel : ObservableObject
    {
        private readonly IIncidentsApi _incidentsApi;
        private readonly IAuthApi _authApi;
        private readonly IResidencesApi _residencesApi;
        private readonly ILotsApi _lotsApi;

        public IncidentsListViewModel(
            IIncidentsApi incidentsApi,
            IAuthApi authApi,
            IResidencesApi residencesApi,
            ILotsApi lotsApi)
        {
            _incidentsApi = incidentsApi;
            _authApi = authApi;
            _residencesApi = residencesApi;
            _lotsApi = lotsApi;

            Items = new();
            Users = new();
            Residences = new();
            Lots = new();
            Urgences = new() { "Tous", "Basse", "Normale", "Elevee", "Critique" };
            SelectedUrgence = "Tous";
        }

        [ObservableProperty] private List<IncidentDto> items;

        // Filtres
        [ObservableProperty] private List<UserDto> users;
        [ObservableProperty] private UserDto? selectedUser;

        [ObservableProperty] private List<ResidenceDto> residences;
        [ObservableProperty] private ResidenceDto? selectedResidence;

        [ObservableProperty] private List<LotDto> lots;
        [ObservableProperty] private LotDto? selectedLot;

        [ObservableProperty] private List<string> urgences;
        [ObservableProperty] private string selectedUrgence;

        [ObservableProperty] private string? searchText; // pour le titre

        [RelayCommand]
        public async Task LoadAsync()
        {
            // 1) Charger incidents
            var data = await _incidentsApi.GetAllAsync();
            var list = data?.OrderByDescending(i => i.DateDeclaration).ToList()
                       ?? new List<IncidentDto>();

            // 2) Charger Users, Résidences, Lots pour filtres + enrichissement
            var usersTask = _authApi.GetAllAsync();
            var residencesTask = _residencesApi.GetAllAsync();
            var lotsTask = _lotsApi.GetAllAsync();

            await Task.WhenAll(usersTask, residencesTask, lotsTask);

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

            Residences = residencesTask.Result?
                .OrderBy(r => r.Nom)
                .ToList() ?? new List<ResidenceDto>();

            Lots = lotsTask.Result?
                .OrderBy(l => l.NumeroLot)
                .ToList() ?? new List<LotDto>();

            // 3) Enrichir les incidents pour l’affichage
            foreach (var inc in list)
            {
                var user = Users.FirstOrDefault(u => u.Id == inc.DeclareParId);
                inc.DeclarantNomComplet = user?.FullName ?? inc.DeclareParId.ToString();

                var res = Residences.FirstOrDefault(r => r.Id == inc.ResidenceId);
                inc.ResidenceNom = res?.Nom;

                var lot = Lots.FirstOrDefault(l => l.Id == inc.LotId);
                inc.LotNumero = lot?.NumeroLot;
            }

            Items = list;
        }

        [RelayCommand]
        private async Task ClearFiltersAsync()
        {
            SelectedUser = null;
            SelectedResidence = null;
            SelectedLot = null;
            SelectedUrgence = null;

            
            await LoadAsync();

        }

        [RelayCommand]
        public async Task FilterAsync()
        {
            var data = await _incidentsApi.GetAllAsync() ?? new List<IncidentDto>();

            // réenrichir rapidement (au minimum pour les filtres)
            var list = data.OrderByDescending(i => i.DateDeclaration).ToList();

            var filtered = list.Where(i =>
                    (string.IsNullOrWhiteSpace(SearchText) ||
                     (i.Titre?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false))
                    && (SelectedUser == null || i.DeclareParId == SelectedUser.Id)
                    && (SelectedResidence == null || i.ResidenceId == SelectedResidence.Id)
                    && (SelectedLot == null || i.LotId == SelectedLot.Id)
                    && (SelectedUrgence == "Tous" || i.Urgence == SelectedUrgence))
                .OrderByDescending(i => i.DateDeclaration)
                .ToList();

            Items = filtered;
        }

        [RelayCommand]
        public Task GoToCreate()
            => Shell.Current.GoToAsync("incident-create");

        [RelayCommand]
        public Task GoToDetails(Guid id)
            => Shell.Current.GoToAsync($"incident-details?id={id}");
    }
}
