using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SyndicApp.Mobile.ViewModels.Finances;


public partial class PaiementsListViewModel : BaseViewModel
{
    public PaiementsListViewModel(IPaiementsApi api) => _api = api;

    public PaiementsListViewModel() : this(ServiceHelper.GetRequiredService<IPaiementsApi>()) { }


    private readonly IPaiementsApi _api;
    [ObservableProperty] private List<object> items = new();

    [RelayCommand] public async Task LoadAsync() { try { IsBusy = true; Items = await _api.GetAll(); } finally { IsBusy = false; } }
}