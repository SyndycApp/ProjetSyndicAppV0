using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SyndicApp.Mobile.ViewModels.Finances;


public partial class SoldesViewModel : BaseViewModel
{
    private readonly ISoldesApi _api;
    [ObservableProperty] private Guid lotId;
    [ObservableProperty] private Guid residenceId;
    [ObservableProperty] private object? soldeLot;
    [ObservableProperty] private object? soldeResidence;


    public SoldesViewModel(ISoldesApi api) => _api = api;

    public SoldesViewModel() : this(ServiceHelper.GetRequiredService<ISoldesApi>()) { }


    [RelayCommand] public async Task LoadLotAsync() => SoldeLot = await _api.SoldeLot(LotId);
    [RelayCommand] public async Task LoadResidenceAsync() => SoldeResidence = await _api.SoldeResidence(ResidenceId);
}