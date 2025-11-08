namespace SyndicApp.Mobile;
using SyndicApp.Mobile.Views.Finances;
using SyndicApp.Mobile.Views.Auth;
using SyndicApp.Mobile.Views;    

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Routes Auth
        Routing.RegisterRoute("forgotcode", typeof(ForgotPasswordPage)); 
        Routing.RegisterRoute("verifycode", typeof(VerifyCodePage));
        Routing.RegisterRoute("resetpwd", typeof(ResetWithCodePage));
        Routing.RegisterRoute("resetpassword", typeof(ResetPasswordPage));
        Routing.RegisterRoute("appel-create", typeof(AppelCreatePage));
        Routing.RegisterRoute("appel-details", typeof(AppelDetailsPage));
        Routing.RegisterRoute("appel-edit", typeof(AppelEditPage));
        Routing.RegisterRoute("appels/list", typeof(AppelsPage));
        Routing.RegisterRoute("drawer", typeof(DrawerPage));

    }
}
