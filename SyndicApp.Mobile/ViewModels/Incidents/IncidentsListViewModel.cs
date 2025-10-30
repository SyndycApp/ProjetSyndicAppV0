namespace SyndicApp.Mobile.ViewModels.Incidents;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class IncidentsListViewModel : BaseViewModel
{
    private readonly IIncidentsApi _api;
    [ObservableProperty] private List<object> items = new();
    public IncidentsListViewModel(IIncidentsApi api) => _api = api;
    public IncidentsListViewModel() : this(ServiceHelper.GetRequiredService<IIncidentsApi>()) { }
    [RelayCommand] public async Task LoadAsync() { try { IsBusy = true; Items = await _api.GetAll(); } finally { IsBusy = false; } }
}