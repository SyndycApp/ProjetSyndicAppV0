using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Finances;

public partial class AppelsListViewModel : ObservableObject
{
    private readonly IAppelsApi _api;

    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private List<AppelDeFondsDto> appels = new();

    public AppelsListViewModel(IAppelsApi api) => _api = api;

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            Appels = await _api.GetAllAsync();
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    public async Task OpenCreateAsync() => await Shell.Current.GoToAsync("appel-create");

    [RelayCommand]
    public async Task OpenDetailsAsync(AppelDeFondsDto a) =>
        await Shell.Current.GoToAsync($"appel-details?id={a.Id}");
}
