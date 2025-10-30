using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SyndicApp.Mobile.ViewModels.Finances;


public partial class AppelsListViewModel : BaseViewModel
{
    private readonly IAppelsApi _api;
    [ObservableProperty] private List<object> items = new();
    public AppelsListViewModel(IAppelsApi api) => _api = api;

    public AppelsListViewModel() : this(ServiceHelper.GetRequiredService<IAppelsApi>()) { }

    [RelayCommand] public async Task LoadAsync() { try { IsBusy = true; Items = await _api.GetAll(); } finally { IsBusy = false; } }
    [RelayCommand] public Task CloturerAsync(Guid id) => _api.Cloturer(id);
}