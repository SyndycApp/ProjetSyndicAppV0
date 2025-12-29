using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;

namespace SyndicApp.Mobile.ViewModels.Personnel;

public partial class PresenceViewModel : ObservableObject
{
    private readonly IPresenceApi _api;

    [ObservableProperty] private bool isWorking;

    public PresenceViewModel(IPresenceApi api)
    {
        _api = api;
    }

    [RelayCommand]
    private async Task StartAsync()
    {
        await _api.StartAsync(new() { ResidenceNom = "Résidence A" });
        IsWorking = true;
    }

    [RelayCommand]
    private async Task EndAsync()
    {
        await _api.EndAsync();
        IsWorking = false;
    }
}
