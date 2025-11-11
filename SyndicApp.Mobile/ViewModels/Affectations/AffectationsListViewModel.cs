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
        }

        // Liste
        [ObservableProperty] private List<AffectationLotDto> items;

        // Recherche + filtres
        [ObservableProperty] private string? searchText;
        [ObservableProperty] private List<UserDto>? users;
        [ObservableProperty] private List<LotDto>? lots;
        [ObservableProperty] private UserDto? selectedUser;
        [ObservableProperty] private LotDto? selectedLot;

        // --- Chargement initial ---
        [RelayCommand]
        public async Task LoadAsync()
        {
            // 1) Affectations
            var data = await _api.GetAllAsync();
            Items = data?.ToList() ?? new List<AffectationLotDto>();

            // 2) Utilisateurs (comme avant)
            try
            {
                var allUsers = await _api.GetAllUsersAsync(); // ApiResult<List<AuthListItemDto>>
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
                    Users = new List<UserDto>();
                }
            }
            catch
            {
                Users = new List<UserDto>();
            }

            // 3) Lots via API (affichage du NumeroLot dans le Picker)
            try
            {
                var allLots = await _lotsApi.GetAllAsync();
                Lots = (allLots ?? new List<LotDto>())
                       .OrderBy(l => l.NumeroLot)
                       .ToList();
            }
            catch
            {
                // Fallback : à partir des Items s’il y a une panne réseau côté Lots
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

        // --- Filtre ---
        [RelayCommand]
        public async Task FilterAsync()
        {
            var data = await _api.GetAllAsync() ?? Enumerable.Empty<AffectationLotDto>();

            Guid? lotIdFilter = null;

            // a) Si un lot est choisi dans le Picker : on résout par numeroLot -> Id
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
                catch { /* non bloquant */ }

                // fallback si resolve-id ne renvoie rien
                if (lotIdFilter == null || lotIdFilter == Guid.Empty)
                    lotIdFilter = SelectedLot.Id;
            }
            // b) Si pas de lot sélectionné mais du texte saisi : tentative de resolve avec le texte
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

                        // Optionnel : sélectionner le lot résolu dans le Picker
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
                catch { /* non bloquant */ }
            }

            // c) Filtre final (on garde tes règles existantes)
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
        public Task GoToCreate() => Shell.Current.GoToAsync("affectation-create");

        [RelayCommand]
        public Task GoToDetails(Guid id) => Shell.Current.GoToAsync($"affectation-details?id={id}");
    }
}
