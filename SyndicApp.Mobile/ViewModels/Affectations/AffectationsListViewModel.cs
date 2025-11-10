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
            var data = await _api.GetAllAsync();
            Items = data?.ToList() ?? new List<AffectationLotDto>();
        }

        // --- Filtre (utilisé par ton bouton "Filtrer") ---
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

        // --- Navigation : noms/params EXACTEMENT comme dans ton XAML ---
        [RelayCommand]
        public Task GoToCreate()
            => Shell.Current.GoToAsync("affectation-create");

        [RelayCommand]
        public Task GoToDetails(Guid id)
            => Shell.Current.GoToAsync($"affectation-details?id={id}");
    }
}
