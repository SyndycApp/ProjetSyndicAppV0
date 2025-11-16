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
    public partial class PaiementsListViewModel : ObservableObject
    {
        private readonly IPaiementsApi _paiementsApi;
        private readonly IAuthApi _authApi;
        private readonly IAppelsApi _appelsApi;

        public PaiementsListViewModel(
          IPaiementsApi paiementsApi,
          IAuthApi authApi,
          IAppelsApi appelsApi)
        {
            _paiementsApi = paiementsApi;
            _authApi = authApi;
            _appelsApi = appelsApi;

            Items = new();
            Users = new();
        }

        [ObservableProperty] private List<PaiementDto> items;

        [ObservableProperty] private List<UserDto>? users;
        [ObservableProperty] private UserDto? selectedUser;
        [ObservableProperty] private DateTime? selectedDate;
        [ObservableProperty] private string? searchText;

        [RelayCommand]
        public async Task LoadAsync()
        {
            // 1) Charger les paiements
            var data = await _paiementsApi.GetAllAsync();
            var list = data?.OrderByDescending(p => p.DatePaiement).ToList()
                       ?? new List<PaiementDto>();

            // 1bis) Enrichir avec la description de l'appel (si API dispo)
            foreach (var p in list)
            {
                if (p.AppelDeFondsId != Guid.Empty)
                {
                    try
                    {
                        var desc = await _appelsApi.GetDescriptionAsync(p.AppelDeFondsId.ToString());
                        if (!string.IsNullOrWhiteSpace(desc))
                            p.AppelDescription = desc;
                    }
                    catch
                    {
                        // En cas d'erreur on laisse l'ID, pas de crash
                        if (string.IsNullOrWhiteSpace(p.AppelDescription))
                            p.AppelDescription = p.AppelDeFondsId.ToString();
                    }
                }
            }

            Items = list;

            // 2) Charger la liste des utilisateurs (inchangé)
            try
            {
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
            catch
            {
                Users = new List<UserDto>();
            }
        }


        [RelayCommand]
        public async Task FilterAsync()
        {
            var data = await _paiementsApi.GetAllAsync() ?? new List<PaiementDto>();

            var filtered = data.Where(x =>
                    (string.IsNullOrWhiteSpace(SearchText) ||
                     (x.NomCompletUser?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false))
                    && (SelectedUser == null || x.UserId == SelectedUser.Id)
                    && (!SelectedDate.HasValue || x.DatePaiement.Date == SelectedDate.Value.Date))
                .OrderByDescending(p => p.DatePaiement)
                .ToList();

            Items = filtered;
        }

        [RelayCommand]
        public Task GoToCreate()
            => Shell.Current.GoToAsync("paiement-create");

        [RelayCommand]
        public Task GoToDetails(Guid id)
            => Shell.Current.GoToAsync($"paiement-details?id={id}");
    }
}
