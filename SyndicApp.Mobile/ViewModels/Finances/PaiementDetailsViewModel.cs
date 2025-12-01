using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Finances
{
    [QueryProperty(nameof(PaiementId), "id")]
    public partial class PaiementDetailsViewModel : ObservableObject
    {
        private readonly IPaiementsApi _paiementsApi;
        private readonly IAppelsApi _appelsApi;
        private readonly IResidencesApi _residencesApi;
        private readonly IAuthApi _authApi;

        public PaiementDetailsViewModel(
            IPaiementsApi paiementsApi,
            IAppelsApi appelsApi,
            IResidencesApi residencesApi,
            IAuthApi authApi)
        {
            _paiementsApi = paiementsApi;
            _appelsApi = appelsApi;
            _residencesApi = residencesApi;
            _authApi = authApi;
        }

        // PARAM ID
        [ObservableProperty] private string? paiementId;

        // Paiement
        [ObservableProperty] private decimal montant;
        [ObservableProperty] private DateTime datePaiement;

        // User
        [ObservableProperty] private string? userFullName;
        [ObservableProperty] private string? userRole;
        [ObservableProperty] private string? userAdresse;

        // Appel
        [ObservableProperty] private string? appelDescription;
        [ObservableProperty] private decimal montantTotal;
        [ObservableProperty] private decimal montantReste;
        [ObservableProperty] private int nbPaiements;
        [ObservableProperty] private DateTime dateEmission;

        // Résidence
        [ObservableProperty] private string? residenceNom;
        [ObservableProperty] private string? residenceAdresseComplete;

        // ===== LOAD =====
        [RelayCommand]
        public async Task LoadAsync()
        {
            if (string.IsNullOrWhiteSpace(PaiementId)) return;
            if (!Guid.TryParse(PaiementId, out var id)) return;

            var paiement = await _paiementsApi.GetByIdAsync(id);

            Montant = paiement.Montant;
            DatePaiement = paiement.DatePaiement;
            UserFullName = paiement.NomCompletUser;

            // User
            var users = await _authApi.GetAllAsync();
            var u = users?.Data?.FirstOrDefault(x => x.Id == paiement.UserId);
            if (u != null)
            {
                UserRole = u.Roles?.FirstOrDefault();
                UserAdresse = u.Email; // placeholder
            }

            // Appel
            var appel = await _appelsApi.GetByIdAsync(paiement.AppelDeFondsId.ToString());
            AppelDescription = appel.Description;
            MontantTotal = appel.MontantTotal;
            MontantReste = appel.MontantReste;
            NbPaiements = appel.NbPaiements;
            DateEmission = appel.DateEmission;
            ResidenceNom = appel.ResidenceNom;

            // Résidence
            var res = await _residencesApi.GetByIdAsync(appel.ResidenceId.ToString());
            ResidenceAdresseComplete = $"{res.Adresse}, {res.Ville} {res.CodePostal}";
        }

        [RelayCommand]
        public Task GoBack() => Shell.Current.GoToAsync("..");
    }
}
