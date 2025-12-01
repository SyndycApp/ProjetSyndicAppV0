using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances;

public partial class ChargeDetailsPage : ContentPage
{
    public ChargeDetailsPage(ChargeDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        // Charge les détails à l'affichage
        Loaded += async (_, __) => await vm.LoadAsync();
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
