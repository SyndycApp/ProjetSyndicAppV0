using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntelliJ.Lang.Annotations;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Affectations
{
    // Shell enverra ?id=... (toujours string) → on le reçoit en string et on parse proprement.
    [QueryProperty(nameof(AffectationId), "id")]
    public partial class AffectationDetailsViewModel : ObservableObject
    {
        private readonly IAffectationsLotsApi _api;

        public AffectationDetailsViewModel(IAffectationsLotsApi api)
        {
            _api = api;
        }

        [ObservableProperty] private AffectationLotDto? item;
        [ObservableProperty] private bool isBusy;

        // Reçoit la valeur de la query ?id=... (toujours string) 
        public string? AffectationId
        {
            get => null; // pas utilisé
            set
            {
                if (Guid.TryParse(value, out var id))
                {
                    // fire-and-forget contrôlé
                    _ = LoadAsync(id);
                }
                else
                {
                    // id invalide → on peut afficher un message si tu veux
                }
            }
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            if (Item?.Id != Guid.Empty)
                await LoadAsync(Item.Id);
        }

        private async Task LoadAsync(Guid id)
        {
            try
            {
                IsBusy = true;
                Item = await _api.GetByIdAsync(id);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
