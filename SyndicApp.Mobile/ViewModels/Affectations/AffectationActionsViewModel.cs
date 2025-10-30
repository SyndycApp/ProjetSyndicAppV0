using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace SyndicApp.Mobile.ViewModels.Affectations;


public partial class AffectationActionsViewModel : BaseViewModel
{
    private readonly IAffectationsLotsApi _api;
    [ObservableProperty] private Guid affectationId;
    [ObservableProperty] private Guid lotId;
    [ObservableProperty] private Guid userId;


    public AffectationActionsViewModel(IAffectationsLotsApi api) => _api = api;

    public AffectationActionsViewModel() : this(ServiceHelper.GetRequiredService<IAffectationsLotsApi>()) { }


    [RelayCommand] public Task CloturerAsync() => _api.Cloturer(AffectationId);
    [RelayCommand] public Task HistoriqueLotAsync() => _api.HistoriqueLot(LotId);
    [RelayCommand] public Task OccupantActuelLotAsync() => _api.OccupantActuelLot(LotId);
    [RelayCommand] public Task AssignerLocataireAsync() => _api.AssignerLocataire(new { lotId = LotId, userId = UserId });
    [RelayCommand] public Task ChangerStatutAsync(string statut) => _api.ChangerStatut(AffectationId, new { statut });
}