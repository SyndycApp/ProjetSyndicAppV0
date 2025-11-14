using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Finances;

public partial class AppelsListViewModel : ObservableObject
{
    private readonly IAppelsApi _api;
    private readonly IResidencesApi _residencesApi;

    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private List<AppelDeFondsDto> appels = new();


    public AppelsListViewModel(IAppelsApi api, IResidencesApi residencesApi)
    {
        _api = api;
        _residencesApi = residencesApi;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            Appels = await _api.GetAllAsync();
            var residences = await _residencesApi.GetAllAsync();
            var lookup = residences.ToDictionary(
                r => r.Id.ToString(),
                r => r.Nom ?? string.Empty);

            foreach (var a in appels)
            {
                if (!string.IsNullOrWhiteSpace(a.ResidenceId)
                    && lookup.TryGetValue(a.ResidenceId, out var nom))
                {
                    a.ResidenceNom = nom;
                }
            }

            Appels = appels;
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    public async Task OpenCreateAsync() => await Shell.Current.GoToAsync("appel-create");

    [RelayCommand]
    public async Task OpenDetailsAsync(AppelDeFondsDto a) =>
        await Shell.Current.GoToAsync($"appel-details?id={a.Id}");
}
