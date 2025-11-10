// using nécessaires
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Affectations
{
    public partial class AffectationsListViewModel : ObservableObject
    {
        private readonly IAffectationsLotsApi _api;

        public AffectationsListViewModel(IAffectationsLotsApi api)
        {
            _api = api;
            Items = new();
            Users = new();   // ← on initialise
            Lots = new();   // ← on initialise
        }

        // Liste
        [ObservableProperty] private List<AffectationLotDto> items;

        // Recherche + filtres (types = tes modèles actuels)
        [ObservableProperty] private string? searchText;
        [ObservableProperty] private List<UserDto>? users;
        [ObservableProperty] private List<LotDto>? lots;
        [ObservableProperty] private UserDto? selectedUser;
        [ObservableProperty] private LotDto? selectedLot;

        // --- Chargement initial ---
        [RelayCommand]
        public async Task LoadAsync()
        {
            // 1) Charger les affectations
            var data = await _api.GetAllAsync();
            Items = data?.ToList() ?? new List<AffectationLotDto>();

            // 2) Remplir la liste des utilisateurs via /api/Auth (mêmes méthodes déjà ajoutées dans IAffectationsLotsApi)
            try
            {
                var allUsers = await _api.GetAllUsersAsync(); // -> ApiResult<List<AuthListItemDto>>
                if (allUsers?.Success == true && allUsers.Data != null)
                {
                    Users = allUsers.Data
                        .Select(u => new UserDto
                        {
                            Id = u.Id,
                            Email = u.Email ?? string.Empty,
                            FullName = !string.IsNullOrWhiteSpace(u.FullName) ? u.FullName! : (u.Email ?? u.Id.ToString()),
                            Roles = u.Roles ?? new List<string>()
                        })
                        .OrderBy(u => u.FullName)
                        .ToList();
                }
                else
                {
                    Users = new List<UserDto>(); // liste vide si rien
                }
            }
            catch
            {
                Users = new List<UserDto>(); // pas de régression si API non joignable
            }

            // 3) Remplir la liste des lots à partir des items chargés (évite d’injecter ILotsApi ici)
            //    -> ça remplit le Picker "Lot" avec ceux présents dans les affectations
            Lots = Items
                .Where(i => i.LotId != Guid.Empty)
                .GroupBy(i => i.LotId)
                .Select(g => new LotDto
                {
                    Id = g.Key,
                    NumeroLot = g.Select(x => x.LotNumero).FirstOrDefault(s => !string.IsNullOrWhiteSpace(s)) ?? g.Key.ToString()
                })
                .OrderBy(l => l.NumeroLot)
                .ToList();
        }

        // --- Filtre (inchangé) ---
        [RelayCommand]
        public async Task FilterAsync()
        {
            var data = await _api.GetAllAsync() ?? Enumerable.Empty<AffectationLotDto>();

            var filtered = data.Where(x =>
                    (string.IsNullOrWhiteSpace(SearchText) ||
                     (x.UserNom?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                     (x.LotNumero?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false))
                    && (SelectedUser == null || x.UserId == SelectedUser.Id)
                    && (SelectedLot == null || x.LotId == SelectedLot.Id))
                .ToList();

            Items = filtered;
        }

        [RelayCommand]
        public Task GoToCreate() => Shell.Current.GoToAsync("affectation-create");

        [RelayCommand]
        public Task GoToDetails(Guid id) => Shell.Current.GoToAsync($"affectation-details?id={id}");
    }
}
