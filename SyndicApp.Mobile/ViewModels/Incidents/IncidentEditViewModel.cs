using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SyndicApp.Mobile.ViewModels.Incidents;


[QueryProperty(nameof(Id), "id")]
public partial class IncidentEditViewModel : BaseViewModel
{
    private readonly IIncidentsApi _api;
    [ObservableProperty] private Guid id;
    [ObservableProperty] private string titre = string.Empty;
    [ObservableProperty] private string description = string.Empty;
    [ObservableProperty] private Guid residenceId;


    public IncidentEditViewModel(IIncidentsApi api) => _api = api;
    public IncidentEditViewModel() : this(ServiceHelper.GetRequiredService<IIncidentsApi>()) { }


    [RelayCommand] public Task CreateAsync() => _api.Create(new { titre = Titre, description = Description, residenceId = ResidenceId });
    [RelayCommand] public Task UpdateAsync() => _api.Update(Id, new { titre = Titre, description = Description });
    [RelayCommand] public Task UpdateStatusAsync(string status) => _api.UpdateStatus(Id, new { status });
}