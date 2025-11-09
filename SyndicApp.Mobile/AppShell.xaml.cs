namespace SyndicApp.Mobile;

using SyndicApp.Mobile.Views;
using SyndicApp.Mobile.Views.Auth;
using SyndicApp.Mobile.Views.Finances;
using SyndicApp.Mobile.Views.Residences;
using SyndicApp.Mobile.Views.Batiments;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // 👉 NE PAS enregistrer ici les routes déjà déclarées en <ShellContent>
        // (login, register, forgot, syndic-dashboard, appels, residences, batiments, drawer)

        // ===== Auth secondaires =====
        Routing.RegisterRoute("forgotcode", typeof(ForgotPasswordPage));
        Routing.RegisterRoute("verifycode", typeof(VerifyCodePage));
        Routing.RegisterRoute("resetpwd", typeof(ResetWithCodePage));
        Routing.RegisterRoute("resetpassword", typeof(ResetPasswordPage));

        // ===== Finances secondaires =====
        Routing.RegisterRoute("appel-create", typeof(AppelCreatePage));
        Routing.RegisterRoute("appel-details", typeof(AppelDetailsPage));
        Routing.RegisterRoute("appel-edit", typeof(AppelEditPage));

        // ===== Résidences secondaires =====
        Routing.RegisterRoute("residence-create", typeof(ResidenceCreatePage));
        Routing.RegisterRoute("residence-details", typeof(ResidenceDetailsPage));
        Routing.RegisterRoute("residence-edit", typeof(ResidenceEditPage));

        // ===== Bâtiments secondaires =====
        Routing.RegisterRoute("batiment-create", typeof(BatimentCreatePage));
        Routing.RegisterRoute("batiment-details", typeof(BatimentDetailsPage));
        Routing.RegisterRoute("batiment-edit", typeof(BatimentEditPage));
    }
}
