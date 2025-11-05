using SyndicApp.Mobile.Services;

namespace SyndicApp.Mobile.ViewModels.Dashboard;

public partial class SyndicDashboardViewModel : ViewModels.Common.BaseViewModel
{
    private readonly TokenStore _tokenStore;

    [ObservableProperty] bool canAddResidence;

    public SyndicDashboardViewModel(TokenStore tokenStore)
    {
        _tokenStore = tokenStore;
        Title = "Dashboard";
        CanAddResidence = _tokenStore.IsSyndic();
    }

    [RelayCommand]
    public async Task GoToAddResidenceAsync()
    {
        if (!CanAddResidence) return;
        await Shell.Current.GoToAsync("add-residence");
    }
}
