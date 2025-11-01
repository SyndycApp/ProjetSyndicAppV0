using SyndicApp.Mobile.ViewModels.Auth;

namespace SyndicApp.Mobile.Views.Auth;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (Shell.Current is not null)
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled; // cache le menu sur Register
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (Shell.Current is not null)
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;   // réactive le menu ensuite
    }
}
