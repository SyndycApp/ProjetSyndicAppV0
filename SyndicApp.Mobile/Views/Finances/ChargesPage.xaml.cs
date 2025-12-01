using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances;

public partial class ChargesPage : ContentPage
{
    public ChargesPage(ChargesListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ChargesListViewModel vm && !vm.IsBusy)
            _ = vm.LoadAsync();
    }
}
