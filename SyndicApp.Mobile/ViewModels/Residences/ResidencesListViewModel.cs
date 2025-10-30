using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;



namespace SyndicApp.Mobile.ViewModels.Residences;


public partial class ResidencesListViewModel : BaseViewModel
{
    private readonly IResidencesApi _api;


    public ResidencesListViewModel(IResidencesApi api) => _api = api;

    public ResidencesListViewModel() : this(ServiceHelper.GetRequiredService<IResidencesApi>()) { }

    [ObservableProperty] private List<object> items = new();


    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try { IsBusy = true; Items = await _api.GetAll(); }
        catch (ApiException ex) { Error = ex.Content ?? ex.Message; }
        finally { IsBusy = false; }
    }
}