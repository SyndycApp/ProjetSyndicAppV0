using SyndicApp.Mobile.ViewModels.Personnel;

namespace SyndicApp.Mobile.Views.Personnel;

public partial class EmployesPage : ContentPage
{
    private readonly EmployesViewModel _vm;

    public EmployesPage(EmployesViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is EmployesViewModel vm)
            await vm.LoadAsync();
    }
}
