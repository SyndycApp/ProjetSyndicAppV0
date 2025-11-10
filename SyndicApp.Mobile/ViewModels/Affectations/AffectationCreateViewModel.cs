using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Affectations
{
    public partial class AffectationCreateViewModel : ObservableObject
    {
        private readonly IAffectationsLotsApi _api;

        public AffectationCreateViewModel(IAffectationsLotsApi api)
        {
            _api = api;
            DateDebut = DateTime.Today;
        }

        // Listes pour les Pickers (types alignés sur tes modèles)
        [ObservableProperty] private List<UserDto>? users;  // remplace si ton type s'appelle autrement
        [ObservableProperty] private List<LotDto>? lots;

        [ObservableProperty] private UserDto? selectedUser; // idem
        [ObservableProperty] private LotDto? selectedLot;

        [ObservableProperty] private DateTime dateDebut;
        [ObservableProperty] private bool estProprietaire;

        [RelayCommand]
        public async Task CreateAsync()
        {
            if (SelectedUser == null || SelectedLot == null)
            {
                await Shell.Current.DisplayAlert("Erreur", "Sélectionne un utilisateur et un lot.", "OK");
                return;
            }

            var dto = new CreateAffectationLotDto
            {
                UserId = SelectedUser.Id,
                LotId = SelectedLot.Id,
                DateDebut = DateDebut,
                EstProprietaire = EstProprietaire
            };

            await _api.CreateAsync(dto);
            await Shell.Current.DisplayAlert("OK", "Affectation créée", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
