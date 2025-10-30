
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SyndicApp.Mobile.ViewModels.Interventions;


public partial class InterventionsListViewModel : BaseViewModel
{
    private readonly IInterventionsApi _api;
    [ObservableProperty] private List<object> items = new();
    public InterventionsListViewModel(IInterventionsApi api) => _api = api;

    public InterventionsListViewModel() : this(ServiceHelper.GetRequiredService<IInterventionsApi>()) { }
    [RelayCommand] public async Task LoadAsync() { try { IsBusy = true; Items = await _api.GetAll(); } finally { IsBusy = false; } }
}