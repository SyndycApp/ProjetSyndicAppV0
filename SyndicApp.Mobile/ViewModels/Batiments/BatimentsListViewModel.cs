using System.Collections.Generic;
using System.Threading.Tasks;
using SyndicApp.Mobile.Api;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SyndicApp.Mobile.ViewModels.Batiments;


public partial class BatimentsListViewModel : BaseViewModel
{
    private readonly IBatimentsApi _api;
    [ObservableProperty] private List<object> items = new();
    public BatimentsListViewModel(IBatimentsApi api) => _api = api;
    public BatimentsListViewModel() : this(ServiceHelper.GetRequiredService<IBatimentsApi>()) { }
    [RelayCommand] public async Task LoadAsync() { try { IsBusy = true; Items = await _api.GetAll(); } finally { IsBusy = false; } }
}