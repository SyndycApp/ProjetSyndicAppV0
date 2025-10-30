using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api; // où se trouve IDevisTravauxApi

namespace SyndicApp.Mobile.ViewModels.Travaux;

public partial class DevisListViewModel : BaseViewModel
{
    private readonly IDevisTravauxApi _api;

    [ObservableProperty] private List<object> items = new();

    public DevisListViewModel(IDevisTravauxApi api) => _api = api;

    public DevisListViewModel() : this(ServiceHelper.GetRequiredService<IDevisTravauxApi>()) { }

    [RelayCommand]
    public async Task LoadAsync()
    {
        try
        {
            IsBusy = true;
            Items = await _api.GetAll();
        }
        finally { IsBusy = false; }
    }

    // ✅ Une commande = un paramètre
    [RelayCommand]
    public Task ApproveAsync(Guid devisId) =>
        _api.Decide(devisId, new { decision = "approve" });

    [RelayCommand]
    public Task RejectAsync(Guid devisId) =>
        _api.Decide(devisId, new { decision = "reject" });
}
