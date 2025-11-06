using SyndicApp.Mobile.ViewModels.Auth;

namespace SyndicApp.Mobile.Views.Auth;

public partial class ForgotPasswordPage : ContentPage
{
    public ForgotPasswordPage(ForgotPasswordViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm; // DI
    }
}
