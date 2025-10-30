using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;


namespace SyndicApp.Mobile.ViewModels.Residences;


[QueryProperty(nameof(Id), "id")]
public partial class ResidenceDetailsViewModel : BaseViewModel
{
    private readonly IResidencesApi _api;
    [ObservableProperty] private Guid id;
    [ObservableProperty] private object? details;
    [ObservableProperty] private List<object> lots = new();


    public ResidenceDetailsViewModel(IResidencesApi api) => _api = api;

    public ResidenceDetailsViewModel() : this(ServiceHelper.GetRequiredService<IResidencesApi>()) { }


    [RelayCommand]
    public async Task LoadAsync()
    {
        try { IsBusy = true; Details = await _api.GetDetails(Id); Lots = await _api.GetLots(Id); }
        catch (ApiException ex) { Error = ex.Content ?? ex.Message; }
        finally { IsBusy = false; }
    }
}