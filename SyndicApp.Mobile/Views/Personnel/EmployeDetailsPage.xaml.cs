using SyndicApp.Mobile.ViewModels.Personnel;

namespace SyndicApp.Mobile.Views.Personnel;

public partial class EmployeDetailsPage : ContentPage
{
    private readonly EmployeDetailsViewModel _vm;

    public EmployeDetailsPage(EmployeDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync();
    }
}
