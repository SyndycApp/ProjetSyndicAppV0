using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances;

public partial class ChargesPage : ContentPage
{
    private readonly ChargesListViewModel _vm;

    public ChargesPage(ChargesListViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync();
    }

    private void OnMenuClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await _vm.NewChargeCommand.ExecuteAsync(null);
    }
}
