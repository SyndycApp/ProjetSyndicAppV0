using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Finances
{
    [QueryProperty(nameof(Id), "id")]
    public partial class AppelEditViewModel : ObservableObject
    {
        private readonly IAppelsApi _api;
        private readonly IResidencesApi _residencesApi;

        [ObservableProperty] private string id = string.Empty;
        [ObservableProperty] private string? description;
        [ObservableProperty] private decimal montantTotal;
        [ObservableProperty] private DateTime dateEmission = DateTime.Today;
        [ObservableProperty] private Guid residenceId;

        [ObservableProperty] private int nbPaiements;
        [ObservableProperty] private decimal montantPaye;
        [ObservableProperty] private decimal montantReste;

        [ObservableProperty] private bool isBusy;

        [ObservableProperty] private List<ResidenceDto> residences = new();
        [ObservableProperty] private ResidenceDto? selectedResidence;

        [ObservableProperty] private bool isSyndic;

        public AppelEditViewModel(IAppelsApi api, IResidencesApi residencesApi)
        {
            _api = api;
            _residencesApi = residencesApi;

            IsSyndic = Preferences.Get("user_role", "").ToLowerInvariant().Contains("syndic");

            _ = LoadResidencesAsync();
        }

        [RelayCommand]
        public async Task LoadResidencesAsync()
        {
            Residences = await _residencesApi.GetAllAsync() ?? new();
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (string.IsNullOrWhiteSpace(Id)) return;

            var dto = await _api.GetByIdAsync(Id);

            Description = dto.Description;
            MontantTotal = dto.MontantTotal;
            DateEmission = dto.DateEmission;
            ResidenceId = dto.ResidenceId;
            NbPaiements = dto.NbPaiements;
            MontantPaye = dto.MontantPaye;
            MontantReste = dto.MontantReste;

            SelectedResidence = Residences.FirstOrDefault(x => x.Id == ResidenceId);
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
            if (!IsSyndic) return;

            var payload = new UpdateAppelDeFondsRequest
            {
                Id = Id,
                Description = Description,
                MontantTotal = MontantTotal,
                DateEmission = DateEmission,
                ResidenceId = ResidenceId,
                NbPaiements = NbPaiements,
                MontantPaye = MontantPaye,
                MontantReste = MontantReste
            };

            await _api.UpdateAsync(Id, payload);

            await Shell.Current.DisplayAlert("Succès", "Modification enregistrée.", "OK");

            await Shell.Current.GoToAsync($"appel-details?id={Id}");
        }
    }
}
