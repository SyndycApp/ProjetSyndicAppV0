using SyndicApp.Mobile.ViewModels.Auth;

namespace SyndicApp.Mobile.Views.Auth;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = ServiceHelper.GetRequiredService<SyndicApp.Mobile.ViewModels.Auth.LoginViewModel>();
    }
}
