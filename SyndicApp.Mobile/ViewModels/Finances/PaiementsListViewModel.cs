using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Finances
{
    public partial class PaiementsListViewModel : ObservableObject
    {
        private readonly IPaiementsApi _paiementsApi;
        private readonly IAuthApi _authApi;
        private readonly IAppelsApi _appelsApi;

        public PaiementsListViewModel(IPaiementsApi paiementsApi, IAuthApi authApi, IAppelsApi appelsApi)
        {
            _paiementsApi = paiementsApi;
            _authApi = authApi;
            _appelsApi = appelsApi;

            Items = new();
            Users = new();

            var role = Preferences.Get("user_role", "").ToLower();
            IsSyndic = role.Contains("syndic");
        }

        // ROLE
        [ObservableProperty] private bool isSyndic;

        // LISTE
        [ObservableProperty] private List<PaiementDto> items;

        // FILTRES
        [ObservableProperty] private List<UserDto>? users;
        [ObservableProperty] private UserDto? selectedUser;
        [ObservableProperty] private DateTime? selectedDate;


        // ===== LOAD =====
        [RelayCommand]
        public async Task LoadAsync()
        {
            var data = await _paiementsApi.GetAllAsync();

            Items = data?
                .OrderByDescending(x => x.DatePaiement)
                .ToList()
                ?? new List<PaiementDto>();

            // load users
            var allUsers = await _authApi.GetAllAsync();
            Users = allUsers?.Data?
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email ?? "",
                    FullName = string.IsNullOrWhiteSpace(u.FullName) ? u.Email : u.FullName,
                    Roles = u.Roles ?? new List<string>()
                })
                .OrderBy(u => u.FullName)
                .ToList();
        }


        // ===== FILTER =====
        [RelayCommand]
        public async Task FilterAsync()
        {
            var data = await _paiementsApi.GetAllAsync() ?? new();

            Items = data.Where(x =>
                    (SelectedUser == null || x.UserId == SelectedUser.Id) &&
                    (!SelectedDate.HasValue || x.DatePaiement.Date == SelectedDate.Value.Date)
                )
                .OrderByDescending(x => x.DatePaiement)
                .ToList();
        }


        // ===== NAV =====
        [RelayCommand]
        public Task GoToCreate()
            => Shell.Current.GoToAsync("paiement-create");

        [RelayCommand]
        public async Task GoToDetailsAsync(Guid id)
        {
            await Shell.Current.GoToAsync($"paiement-details?id={id}");
        }

    }
}
