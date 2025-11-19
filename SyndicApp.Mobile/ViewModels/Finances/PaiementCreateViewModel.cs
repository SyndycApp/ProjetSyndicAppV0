using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Finances
{
    public partial class PaiementCreateViewModel : ObservableObject
    {
        private readonly IPaiementsApi _paiementsApi;
        private readonly IAppelsApi _appelsApi;
        private readonly IAuthApi _authApi;

        public PaiementCreateViewModel(IPaiementsApi paiementsApi, IAppelsApi appelsApi, IAuthApi authApi)
        {
            _paiementsApi = paiementsApi;
            _appelsApi = appelsApi;
            _authApi = authApi;

            Appels = new();
            Users = new();
            DatePaiement = DateTime.Today;
        }

        [ObservableProperty] private List<AppelDeFondsDto> appels;
        [ObservableProperty] private AppelDeFondsDto? selectedAppel;

        [ObservableProperty] private List<UserDto> users;
        [ObservableProperty] private UserDto? selectedUser;

        [ObservableProperty] private decimal montant;
        [ObservableProperty] private DateTime datePaiement;

        [RelayCommand]
        public async Task LoadAsync()
        {
            // Appels de fonds
            var appelsList = await _appelsApi.GetAllAsync();
            Appels = appelsList?.OrderByDescending(a => a.DateEmission).ToList() ?? new List<AppelDeFondsDto>();

            // Utilisateurs
            var allUsers = await _authApi.GetAllAsync();
            if (allUsers?.Success == true && allUsers.Data != null)
            {
                Users = allUsers.Data
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
            if (SelectedAppel == null || SelectedUser == null)
            {
                await Shell.Current.DisplayAlert("Erreur",
                    "Veuillez sélectionner l'appel de fonds et l'utilisateur.",
                    "OK");
                return;
            }

            if (Montant <= 0)
            {
                await Shell.Current.DisplayAlert("Erreur",
                    "Le montant doit être supérieur à 0.",
                    "OK");
                return;
            }

            try
            {
                var appelId = Guid.Parse(SelectedAppel.Id.ToString()!);
                var userId = Guid.Parse(SelectedUser.Id.ToString()!);
                var request = new PaiementCreateRequest
                {
                    AppelDeFondsId = appelId,  
                    UserId = userId,         
                    Montant = Montant,
                    DatePaiement = DatePaiement
                };

                await _paiementsApi.CreateAsync(request);

                await Shell.Current.DisplayAlert("Succès", "Paiement enregistré.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (ApiException ex)
            {
                var details = string.IsNullOrWhiteSpace(ex.Content) ? ex.Message : ex.Content;
                await Shell.Current.DisplayAlert("Erreur API (400)", details, "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
        }

    }
}
