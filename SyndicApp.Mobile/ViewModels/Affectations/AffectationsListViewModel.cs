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
        private readonly ILotsApi _lotsApi;

        public AffectationsListViewModel(IAffectationsLotsApi api, ILotsApi lotsApi)
        {
            _api = api;
            _lotsApi = lotsApi;

            Items = new();
            Users = new();
            Lots = new();

            CanCreate = true;
        }

        [ObservableProperty] private List<AffectationLotDto> items;

        [ObservableProperty] private string? searchText;
        [ObservableProperty] private List<UserDto>? users;
        [ObservableProperty] private List<LotDto>? lots;
        [ObservableProperty] private UserDto? selectedUser;
        [ObservableProperty] private LotDto? selectedLot;

        [ObservableProperty] private bool canCreate;

        [RelayCommand]
        public async Task LoadAsync()
        {
            var data = await _api.GetForCurrentUserAsync();
            Items = data?.ToList() ?? new List<AffectationLotDto>();

            try
            {
                var allUsers = await _api.GetAllUsersAsync();
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

            try
            {
                var allLots = await _lotsApi.GetAllAsync();
                Lots = (allLots ?? new List<LotDto>())
                       .OrderBy(l => l.NumeroLot)
                       .ToList();
            }
            catch
            {
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
        }

        [RelayCommand]
        public async Task FilterAsync()
        {
            var data = await _api.GetForCurrentUserAsync() ?? Enumerable.Empty<AffectationLotDto>();

            Guid? lotIdFilter = null;

            if (SelectedLot != null)
            {
                try
                {
                    var numero = SelectedLot.NumeroLot?.Trim();
                    if (!string.IsNullOrWhiteSpace(numero))
                    {
                        var hits = await _lotsApi.ResolveManyAsync(numeroLot: numero, type: null);
                        var first = hits?.FirstOrDefault();
                        if (first != null && first.Id != Guid.Empty)
                            lotIdFilter = first.Id;
                    }
                }
                catch { }

                if (lotIdFilter == null || lotIdFilter == Guid.Empty)
                    lotIdFilter = SelectedLot.Id;
            }
            else if (!string.IsNullOrWhiteSpace(SearchText))
            {
                try
                {
                    var txt = SearchText.Trim();
                    var hits = await _lotsApi.ResolveManyAsync(numeroLot: txt, type: null);
                    if (hits == null || hits.Count == 0)
                        hits = await _lotsApi.ResolveManyAsync(numeroLot: null, type: txt);

                    var first = hits?.FirstOrDefault();
                    if (first != null)
                    {
                        lotIdFilter = first.Id;

                        var lot = Lots?.FirstOrDefault(l => l.Id == first.Id)
                                  ?? new LotDto { Id = first.Id, NumeroLot = first.NumeroLot ?? txt };

                        if (Lots != null && Lots.All(x => x.Id != lot.Id))
                        {
                            var cur = Lots.ToList();
                            cur.Add(lot);
                            Lots = cur;
                        }
                        SelectedLot = lot;
                    }
                }
                catch { }
            }

            var filtered = data.Where(x =>
                    (string.IsNullOrWhiteSpace(SearchText) ||
                     (x.UserNom?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                     (x.LotNumero?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false))
                    && (SelectedUser == null || x.UserId == SelectedUser.Id)
                    && (lotIdFilter == null || x.LotId == lotIdFilter.Value))
                .ToList();

            Items = filtered;
        }

        [RelayCommand]
        public async Task GoToCreate()
        {
            if (!CanCreate)
            {
                await Shell.Current.DisplayAlert("Droits insuffisants",
                    "Tu n'as pas le droit de créer une affectation.", "OK");
                return;
            }

            await Shell.Current.GoToAsync("affectation-create");
        }

        [RelayCommand]
        public Task GoToDetails(Guid id) => Shell.Current.GoToAsync($"affectation-details?id={id}");
    }
}
