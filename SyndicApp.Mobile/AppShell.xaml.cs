namespace SyndicApp.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Routes Auth
        Routing.RegisterRoute("forgotcode", typeof(Views.Auth.ForgotPasswordPage)); 
        Routing.RegisterRoute("verifycode", typeof(Views.Auth.VerifyCodePage));
        Routing.RegisterRoute("resetpwd", typeof(Views.Auth.ResetWithCodePage));
        Routing.RegisterRoute("resetpassword", typeof(Views.Auth.ResetPasswordPage));
    }
}
