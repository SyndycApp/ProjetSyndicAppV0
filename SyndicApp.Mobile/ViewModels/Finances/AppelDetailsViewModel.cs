using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Finances;

// l’URI passe ?id=... (toujours string côté Shell)
[QueryProperty(nameof(Id), "id")]
public partial class AppelDetailsViewModel : ObservableObject
{
    private readonly IAppelsApi _api;

    [ObservableProperty] private string id = string.Empty;     // ← Guid → string
    [ObservableProperty] private AppelDeFondsDto? appel;
    [ObservableProperty] private bool isBusy;

    public AppelDetailsViewModel(IAppelsApi api) => _api = api;

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy || string.IsNullOrWhiteSpace(Id)) return;   // ← check string
        try
        {
            IsBusy = true;
            Appel = await _api.GetByIdAsync(Id);
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    public async Task EditAsync() => await Shell.Current.GoToAsync($"appel-edit?id={Id}");

    [RelayCommand]
    public async Task CloturerAsync()
    {
        if (IsBusy || string.IsNullOrWhiteSpace(Id)) return;
        try
        {
            IsBusy = true;
            Appel = await _api.CloturerAsync(Id);
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    public async Task DeleteAsync()
    {
        if (string.IsNullOrWhiteSpace(Id)) return;
        var ok = await Shell.Current.DisplayAlert("Suppression", "Supprimer cet appel ?", "Oui", "Non");
        if (!ok) return;

        await _api.DeleteAsync(Id);
        await Shell.Current.GoToAsync("..");
    }
}
