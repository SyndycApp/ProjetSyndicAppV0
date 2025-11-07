using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Finances;

public partial class AppelCreateViewModel : ObservableObject
{
    private readonly IAppelsApi _api;

    [ObservableProperty] private string? titre;
    [ObservableProperty] private string? description;
    [ObservableProperty] private decimal montantTotal;
    [ObservableProperty] private DateTime dateEmission = DateTime.Today;
    [ObservableProperty] private string residenceId = string.Empty;   // Guid -> string

    [ObservableProperty] private bool isBusy;

    public AppelCreateViewModel(IAppelsApi api) => _api = api;

    [RelayCommand]
    public async Task CreateAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var dto = new AppelDeFondsDto
            {
                Description = string.IsNullOrWhiteSpace(Titre) ? Description : Titre,
                MontantTotal = MontantTotal,
                DateEmission = DateEmission,
                ResidenceId = ResidenceId                               // string
            };
            var created = await _api.CreateAsync(dto);                  // returns dto with string Id
            await Shell.Current.GoToAsync($"appel-details?id={created.Id}");
        }
        finally { IsBusy = false; }
    }
}
