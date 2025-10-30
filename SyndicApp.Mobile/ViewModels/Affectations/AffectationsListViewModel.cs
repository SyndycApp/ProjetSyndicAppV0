using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace SyndicApp.Mobile.ViewModels.Affectations;


public partial class AffectationsListViewModel : BaseViewModel
{
    private readonly IAffectationsLotsApi _api;
    [ObservableProperty] private List<object> items = new();
    public AffectationsListViewModel(IAffectationsLotsApi api) => _api = api;

    public AffectationsListViewModel() : this(ServiceHelper.GetRequiredService<IAffectationsLotsApi>()) { }


    [RelayCommand] public async Task LoadAsync() { try { IsBusy = true; Items = await _api.GetAll(); } finally { IsBusy = false; } }
}