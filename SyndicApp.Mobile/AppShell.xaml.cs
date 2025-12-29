namespace SyndicApp.Mobile;

using SyndicApp.Mobile.Services.AppelVocal;
using SyndicApp.Mobile.Views;
using SyndicApp.Mobile.Views.Affectations;
using SyndicApp.Mobile.Views.AppelVocal;
using SyndicApp.Mobile.Views.Auth;
using SyndicApp.Mobile.Views.Batiments;
using SyndicApp.Mobile.Views.Communication;
using SyndicApp.Mobile.Views.Dashboard;
using SyndicApp.Mobile.Views.Finances;
using SyndicApp.Mobile.Views.Incidents;
using SyndicApp.Mobile.Views.Lots;
using SyndicApp.Mobile.Views.Personnel;
using SyndicApp.Mobile.Views.Residences;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // =========================================================
        // ⚠️ NE PAS déclarer ici les routes déjà en <ShellContent>
        // =========================================================

        // ===== Auth secondaires =====
        Routing.RegisterRoute("forgotcode", typeof(ForgotPasswordPage));
        Routing.RegisterRoute("verifycode", typeof(VerifyCodePage));
        Routing.RegisterRoute("resetpwd", typeof(ResetWithCodePage));
        Routing.RegisterRoute("resetpassword", typeof(ResetPasswordPage));

        // ===== Finances =====
        Routing.RegisterRoute("appel-create", typeof(AppelCreatePage));
        Routing.RegisterRoute("appel-details", typeof(AppelDetailsPage));
        Routing.RegisterRoute("appel-edit", typeof(AppelEditPage));

        Routing.RegisterRoute("charge-create", typeof(ChargeCreatePage));
        Routing.RegisterRoute("charge-edit", typeof(ChargeEditPage));
        Routing.RegisterRoute("charge-details", typeof(ChargeDetailsPage));

        Routing.RegisterRoute("paiement-create", typeof(PaiementCreatePage));
        Routing.RegisterRoute("paiement-details", typeof(PaiementDetailsPage));

        // ===== Résidences =====
        Routing.RegisterRoute("residence-create", typeof(ResidenceCreatePage));
        Routing.RegisterRoute("residence-details", typeof(ResidenceDetailsPage));
        Routing.RegisterRoute("residence-edit", typeof(ResidenceEditPage));

        // ===== Bâtiments =====
        Routing.RegisterRoute("batiment-create", typeof(BatimentCreatePage));
        Routing.RegisterRoute("batiment-details", typeof(BatimentDetailsPage));
        Routing.RegisterRoute("batiment-edit", typeof(BatimentEditPage));

        // ===== Lots =====
        Routing.RegisterRoute("lot-create", typeof(LotCreatePage));
        Routing.RegisterRoute("lot-details", typeof(LotDetailsPage));
        Routing.RegisterRoute("lot-edit", typeof(LotEditPage));

        // ===== Affectations =====
        Routing.RegisterRoute("affectation-create", typeof(AffectationCreatePage));
        Routing.RegisterRoute("affectation-details", typeof(AffectationDetailsPage));
        Routing.RegisterRoute("affectation-historique", typeof(AffectationHistoriquePage));

        // ===== Incidents & travaux =====
        Routing.RegisterRoute("incident-create", typeof(IncidentCreatePage));
        Routing.RegisterRoute("incident-details", typeof(IncidentDetailsPage));
        Routing.RegisterRoute("incident-edit", typeof(IncidentEditPage));
        Routing.RegisterRoute("incident-status", typeof(IncidentStatusPage));

        Routing.RegisterRoute("devis-create", typeof(DevisTravauxCreatePage));
        Routing.RegisterRoute("devis-details", typeof(DevisTravauxDetailsPage));
        Routing.RegisterRoute("devis-decision", typeof(DevisTravauxDecisionPage));

        Routing.RegisterRoute("intervention-details", typeof(InterventionDetailsPage));

        // ===== Prestataires =====
        Routing.RegisterRoute("prestataire-create", typeof(PrestataireCreatePage));
        Routing.RegisterRoute("prestataire-details", typeof(PrestataireDetailsPage));

        // ===== Messagerie =====
        Routing.RegisterRoute("chat", typeof(ChatPage));
        Routing.RegisterRoute("conversations", typeof(ConversationsPage));
        Routing.RegisterRoute("new-conversation", typeof(NewConversationPage));

        // ===== Appel vocal =====
        Routing.RegisterRoute("active-call", typeof(ActiveCallPage));
        Routing.RegisterRoute("incoming-call", typeof(IncomingCallPage));

        // =========================================================
        // 👷 PERSONNEL (CORRIGÉ SANS RÉGRESSION)
        // =========================================================
        Routing.RegisterRoute("personnel/plannings", typeof(PlanningsPage));
        Routing.RegisterRoute("personnel/planning-details", typeof(PlanningPresencePage));
        Routing.RegisterRoute("personnel/presence", typeof(PresencePage));
        Routing.RegisterRoute("personnel/employe-details", typeof(EmployeDetailsPage));
        Routing.RegisterRoute("personnel/employes", typeof(EmployesPage));

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (IncomingCallState.CallId != null)
        {
            var callId = IncomingCallState.CallId.Value;
            var callerId = IncomingCallState.CallerId!.Value;

            IncomingCallState.CallId = null;
            IncomingCallState.CallerId = null;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync(
                    "incoming-call",
                    new Dictionary<string, object>
                    {
                        ["CallId"] = callId,
                        ["CallerId"] = callerId
                    }
                );
            });
        }
    }
}
