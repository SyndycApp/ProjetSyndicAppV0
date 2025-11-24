using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Affectations
{
    public partial class AffectationCreateViewModel : ObservableObject
    {
        private readonly IAffectationsLotsApi _api;
        private readonly ILotsApi _lotsApi;

        public AffectationCreateViewModel(IAffectationsLotsApi api, ILotsApi lotsApi)
        {
            _api = api;
            _lotsApi = lotsApi;

            DateDebut = DateTime.Today;

            // TODO: à piloter selon le rôle (Syndic, etc.)
            CanCreate = true;
        }

        [ObservableProperty] private List<UserSelectItem>? users;
        [ObservableProperty] private List<LotDto>? lots;

        [ObservableProperty] private UserSelectItem? selectedUser;
        [ObservableProperty] private LotDto? selectedLot;

        [ObservableProperty] private DateTime dateDebut;
        [ObservableProperty] private bool estProprietaire;

        [ObservableProperty] private bool canCreate;

        [RelayCommand]
        public async Task LoadAsync()
        {
            try
            {

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
                    var msg = all?.Errors != null ? string.Join(", ", all.Errors) : "Réponse vide";
                    await Shell.Current.DisplayAlert("Users", $"Aucun utilisateur. Détails: {msg}", "OK");
                }


                Lots = await _lotsApi.GetAllAsync();
                if (Lots == null || Lots.Count == 0)
                    await Shell.Current.DisplayAlert("Lots", "Aucun lot retourné.", "OK");
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

        [RelayCommand]
        public async Task CreateAsync()
        {
            if (!CanCreate)
            {
                await Shell.Current.DisplayAlert("Droits insuffisants",
                    "Tu n'as pas le droit de créer une affectation.", "OK");
                return;
            }

            if (SelectedUser == null || SelectedLot == null)
            {
                await Shell.Current.DisplayAlert("Erreur", "Sélectionne un utilisateur et un lot.", "OK");
                return;
            }

            var resolvedUserId = await ResolveUserIdAsync(SelectedUser);
            if (resolvedUserId == null)
            {
                await Shell.Current.DisplayAlert("Erreur", "Impossible de récupérer l'identifiant de l'utilisateur.", "OK");
                return;
            }

            var dto = new CreateAffectationLotDto
            {
                UserId = resolvedUserId.Value,
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
