using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SyndicApp.Mobile.ViewModels.Finances;


public partial class ChargesListViewModel : BaseViewModel
{
    private readonly IChargesApi _api;
    [ObservableProperty] private List<object> items = new();
    public ChargesListViewModel(IChargesApi api) => _api = api;

    public ChargesListViewModel() : this(ServiceHelper.GetRequiredService<IChargesApi>()) { }
    [RelayCommand] public async Task LoadAsync() { try { IsBusy = true; Items = await _api.GetAll(); } finally { IsBusy = false; } }
}