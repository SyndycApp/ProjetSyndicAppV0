using Refit;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace SyndicApp.Mobile.ViewModels.Lots;


public partial class LotsListViewModel : BaseViewModel
{
    private readonly ILotsApi _api;
    [ObservableProperty] private List<object> items = new();
    public LotsListViewModel(ILotsApi api) => _api = api;

    public LotsListViewModel() : this(ServiceHelper.GetRequiredService<ILotsApi>()) { }
    [RelayCommand]
    public async Task LoadAsync()
    {
        try { IsBusy = true; Items = await _api.GetAll(); }
        catch (ApiException ex) { Error = ex.Content ?? ex.Message; }
        finally { IsBusy = false; }
    }
}