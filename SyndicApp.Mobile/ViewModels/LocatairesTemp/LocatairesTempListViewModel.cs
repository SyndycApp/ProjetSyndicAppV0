
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SyndicApp.Mobile.ViewModels.LocatairesTemp;


public partial class LocatairesTempListViewModel : BaseViewModel
{
    private readonly ILocatairesTemporairesApi _api;
    [ObservableProperty] private List<object> items = new();
    public LocatairesTempListViewModel(ILocatairesTemporairesApi api) => _api = api;

    public LocatairesTempListViewModel() : this(ServiceHelper.GetRequiredService<ILocatairesTemporairesApi>()) { }
    [RelayCommand] public async Task LoadAsync() { try { IsBusy = true; Items = await _api.GetAll(); } finally { IsBusy = false; } }
}