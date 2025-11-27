// SyndicApp.Mobile/ViewModels/Affectations/AffectationCreateViewModel.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Affectations
{
    // permet de recevoir ?id=... quand on est en mode édition
    [QueryProperty(nameof(IdParam), "id")]
    public partial class AffectationCreateViewModel : ObservableObject
    {
        private readonly IAffectationsLotsApi _api;
        private readonly ILotsApi _lotsApi;

        public AffectationCreateViewModel(IAffectationsLotsApi api, ILotsApi lotsApi)
        {
            _api = api;
            _lotsApi = lotsApi;

            DateDebut = DateTime.Today;
            CanCreate = true;
        }

        // --- Navigation / édition ---
        [ObservableProperty] private string? idParam;
        [ObservableProperty] private Guid id;

        // True si on est en mode édition
        public bool IsEdit => Id != Guid.Empty;

        // --- Données pour les pickers ---
        [ObservableProperty] private List<UserSelectItem>? users;
        [ObservableProperty] private List<LotDto>? lots;

        [ObservableProperty] private UserSelectItem? selectedUser;
        [ObservableProperty] private LotDto? selectedLot;

        // --- Champs d’édition ---
        [ObservableProperty] private DateTime dateDebut;
        [ObservableProperty] private DateTime? dateFin;
        [ObservableProperty] private bool estProprietaire;

        [ObservableProperty] private bool canCreate;

        [RelayCommand]
        public async Task LoadAsync()
        {
            try
            {
                // si on a ?id=... on le stocke
                if (!string.IsNullOrWhiteSpace(IdParam) &&
                    Guid.TryParse(IdParam, out var gid))
                {
                    Id = gid;
                }

                // Charger les users
                var all = await _api.GetAllUsersAsync();
                if (all?.Success == true && all.Data != null)
                {
                    Users = all.Data
                        .Select(u => new UserSelectItem
                        {
                            Id = u.Id,
                            Email = u.Email,
                            Roles = u.Roles,
                            Label = !string.IsNullOrWhiteSpace(u.FullName)
                                ? u.FullName!
                                : (u.Email ?? u.Id.ToString())
                        })
                        .OrderBy(u => u.Label)
                        .ToList();
                }
                else
                {
                    Users = new List<UserSelectItem>();
                }

                // Charger les lots
                Lots = await _lotsApi.GetAllAsync() ?? new List<LotDto>();

                // Si édition : récupérer l’affectation pour pré-remplir
                if (IsEdit)
                {
                    var current = await _api.GetByIdAsync(Id);
                    if (current != null)
                    {
                        DateDebut = current.DateDebut;
                        DateFin = current.DateFin;
                        EstProprietaire = current.EstProprietaire;

                        // pré-sélectionner user
                        if (Users != null && current.UserId != Guid.Empty)
                        {
                            SelectedUser = Users
                                .FirstOrDefault(u => u.Id == current.UserId);
                        }

                        // pré-sélectionner lot
                        if (Lots != null && current.LotId != Guid.Empty)
                        {
                            SelectedLot = Lots
                                .FirstOrDefault(l => l.Id == current.LotId);
                        }
                    }
                }
            }
            catch (ApiException apiEx)
            {
                await Shell.Current.DisplayAlert("API Error",
                    $"{apiEx.StatusCode} - {apiEx.Content}", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur",
                    ex.Message, "OK");
            }
        }

        private async Task<Guid?> ResolveUserIdAsync(UserSelectItem item)
        {
            var label = item.Label?.Trim();
            if (string.IsNullOrWhiteSpace(label))
                return item.Id;

            var hits = await _api.LookupUsersAsync(q: label, role: null, take: 10);

            var exact = hits.FirstOrDefault(x =>
                string.Equals(x.Label?.Trim(), label, StringComparison.OrdinalIgnoreCase));

            if (exact != null) return exact.Id;

            return hits.FirstOrDefault()?.Id ?? item.Id;
        }

        // appelé par le bouton "Enregistrer" (OnSaveClicked → vm.CreateAsync())
        [RelayCommand]
        public async Task CreateAsync()
        {
            if (!CanCreate)
            {
                await Shell.Current.DisplayAlert("Droits insuffisants",
                    "Tu n'as pas le droit de créer/modifier une affectation.", "OK");
                return;
            }

            if (SelectedUser == null || SelectedLot == null)
            {
                await Shell.Current.DisplayAlert("Erreur",
                    "Sélectionne un utilisateur et un lot.", "OK");
                return;
            }

            var resolvedUserId = await ResolveUserIdAsync(SelectedUser);
            if (resolvedUserId == null)
            {
                await Shell.Current.DisplayAlert("Erreur",
                    "Impossible de récupérer l'identifiant de l'utilisateur.", "OK");
                return;
            }

            try
            {
                if (IsEdit)
                {
                    // ----- MODE ÉDITION → PUT -----
                    var dtoUpdate = new UpdateAffectationLotDto
                    {
                        DateDebut = DateDebut,
                        DateFin = DateFin,
                        EstProprietaire = EstProprietaire
                    };

                    // si ton IAffectationsLotsApi renvoie un body
                    await _api.UpdateAsync(Id, dtoUpdate);
                    // si l’API renvoie 204, Refit va lever une ApiException → gérée plus bas
                }
                else
                {
                    // ----- MODE CRÉATION → POST -----
                    var dtoCreate = new CreateAffectationLotDto
                    {
                        UserId = resolvedUserId.Value,
                        LotId = SelectedLot.Id,
                        DateDebut = DateDebut,
                        EstProprietaire = EstProprietaire
                    };

                    await _api.CreateAsync(dtoCreate);
                }

                await Shell.Current.DisplayAlert("OK",
                    IsEdit ? "Affectation mise à jour." : "Affectation créée.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (ApiException apiEx) when (apiEx.StatusCode == HttpStatusCode.NoContent)
            {
                // ✅ 204 = succès sans body → on considère que tout s’est bien passé
                await Shell.Current.DisplayAlert("OK",
                    IsEdit ? "Affectation mise à jour." : "Affectation créée.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (ApiException apiEx)
            {
                await Shell.Current.DisplayAlert("API Error",
                    $"{(int)apiEx.StatusCode} - {apiEx.StatusCode}\n{apiEx.Content}", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur",
                    ex.Message, "OK");
            }
        }
    }
}
