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

        public PaiementsListViewModel(IPaiementsApi paiementsApi, IAuthApi authApi)
        {
            _paiementsApi = paiementsApi;
            _authApi = authApi;

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
            var data = await _paiementsApi.GetAllAsync();
            Items = data?.OrderByDescending(p => p.DatePaiement).ToList() ?? new List<PaiementDto>();

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
