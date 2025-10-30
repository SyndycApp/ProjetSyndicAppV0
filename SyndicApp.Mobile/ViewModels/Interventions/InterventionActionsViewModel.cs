using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SyndicApp.Mobile.ViewModels.Interventions;


[QueryProperty(nameof(Id), "id")]
public partial class InterventionActionsViewModel : BaseViewModel
{
    private readonly IInterventionsApi _api;
    [ObservableProperty] private Guid id;


    public InterventionActionsViewModel(IInterventionsApi api) => _api = api;

    public InterventionActionsViewModel() : this(ServiceHelper.GetRequiredService<IInterventionsApi>()) { }


    [RelayCommand] public Task UpdateStatusAsync(string status) => _api.UpdateStatus(Id, new { status });
    [RelayCommand] public Task DeleteAsync() => _api.Delete(Id);
}