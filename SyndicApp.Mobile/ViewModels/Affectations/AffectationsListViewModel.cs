using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Affectations
{
    public partial class AffectationsListViewModel : ObservableObject
    {
        private readonly IAffectationsLotsApi _api;
        private readonly IAuthApi _authApi;
        private readonly ILotsApi _lotsApi;

        public AffectationsListViewModel(
            IAffectationsLotsApi api,
            IAuthApi authApi,
            ILotsApi lotsApi)
        {
            _api = api;
            _authApi = authApi;
            _lotsApi = lotsApi;

            Items = new();
            Users = new();
        }

        [ObservableProperty] private List<AffectationLotDto> items;
        [ObservableProperty] private List<UserDto> users;
        [ObservableProperty] private UserDto? selectedUser;

        // CHARGEMENT INITIAL
        [RelayCommand]
        public async Task LoadAsync()
        {
            var data = await _api.GetForCurrentUserAsync();
            var list = data?.ToList() ?? new List<AffectationLotDto>();

            // 1️⃣ Charger tous les utilisateurs depuis /api/Auth
            var authResponse = await _authApi.GetAllAsync();
            var allUsers = authResponse.Data ?? new List<UserDto>();

            // 2️⃣ Enrichir les données manquantes
            foreach (var item in list)
            {
                // USER NOM
                var user = allUsers.FirstOrDefault(u => u.Id == item.UserId);
                item.UserNom = user?.FullName ?? user?.Email ?? "Utilisateur";

                // LOT NUMERO
                try
                {
                    var lot = await _lotsApi.GetByIdAsync(item.LotId);
                    item.LotNumero = lot?.NumeroLot ?? "Lot";
                }
                catch
                {
                    item.LotNumero = "Lot";
                }
            }

            Items = list;

            // 3️⃣ Préparer la liste des users pour le Picker
            Users = allUsers
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.FullName ?? u.Email!
                })
                .OrderBy(u => u.FullName)
                .ToList();
        }

        // FILTRE UTILISATEUR
        [RelayCommand]
        public async Task FilterAsync()
        {
            var data = await _api.GetForCurrentUserAsync();

            var filtered = data?
                .Where(x => SelectedUser == null || x.UserId == SelectedUser.Id)
                .ToList()
                ?? new List<AffectationLotDto>();

            Items = filtered;
        }

        [RelayCommand]
        public Task GoToCreate() =>
            Shell.Current.GoToAsync("affectation-create");

        [RelayCommand]
        public Task GoToDetails(Guid id) =>
            Shell.Current.GoToAsync($"affectation-details?id={id}");
    }
}
