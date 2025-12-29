using Microcharts.Maui;
using Plugin.Maui.Audio;
using SyndicApp.Mobile.Config;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Api.Communication;
using SyndicApp.Mobile.Handlers;
using SyndicApp.Mobile.Services;
using SyndicApp.Mobile.Services.AppelVocal;
using SyndicApp.Mobile.Services.Communication;
using SyndicApp.Mobile.ViewModels.Affectations;
using SyndicApp.Mobile.ViewModels.AppelVocal;
using SyndicApp.Mobile.ViewModels.Auth;
using SyndicApp.Mobile.ViewModels.Batiments;
using SyndicApp.Mobile.ViewModels.Communication;
using SyndicApp.Mobile.ViewModels.Dashboard;
using SyndicApp.Mobile.ViewModels.Finances;
using SyndicApp.Mobile.ViewModels.Incidents;
using SyndicApp.Mobile.ViewModels.Lots;
using SyndicApp.Mobile.ViewModels.Personnel;
using SyndicApp.Mobile.ViewModels.Residences;
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
using System.Text.Json;

namespace SyndicApp.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMicrocharts();

        const string BaseUrl = AppConfig.ApiBaseUrl;

        // =========================
        // 🔧 JSON / Refit
        // =========================
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var refitSettings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(jsonOptions)
        };

        // =========================
        // 🔐 Stores & Handlers
        // =========================
        builder.Services.AddSingleton<TokenStore>();
        builder.Services.AddTransient<AuthHeaderHandler>();

        // =========================
        // 🔓 APIs PUBLIQUES
        // =========================
        builder.Services.AddRefitClient<IAuthApi>(refitSettings)
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(BaseUrl);
                c.Timeout = TimeSpan.FromSeconds(60);
            });

        builder.Services.AddRefitClient<IPasswordApi>(refitSettings)
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(BaseUrl);
                c.Timeout = TimeSpan.FromSeconds(60);
            });

        // =========================
        // 🔐 Helper API sécurisées
        // =========================
        void AddSecured<T>() where T : class =>
            builder.Services.AddRefitClient<T>(refitSettings)
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(BaseUrl);
                    c.Timeout = TimeSpan.FromSeconds(60);
                })
                .AddHttpMessageHandler<AuthHeaderHandler>();

        // =========================
        // 📞 Call API
        // =========================
        builder.Services.AddRefitClient<ICallApi>(refitSettings)
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(BaseUrl);
                c.Timeout = TimeSpan.FromSeconds(60);
            })
            .AddHttpMessageHandler<AuthHeaderHandler>();


        // =========================
        // 🔐 APIs sécurisées
        // =========================
        AddSecured<IChatApi>();
        AddSecured<IResidencesApi>();
        AddSecured<IBatimentsApi>();
        AddSecured<ILotsApi>();
        AddSecured<IUsersApi>();
        AddSecured<IAffectationsLotsApi>();
        AddSecured<IAffectationLotsApiAlt>();
        AddSecured<IChargesApi>();
        AddSecured<IAppelsApi>();
        AddSecured<IPaiementsApi>();
        AddSecured<IIncidentsApi>();
        AddSecured<IDevisTravauxApi>();
        AddSecured<IInterventionsApi>();
        AddSecured<IPrestatairesApi>();
        AddSecured<IConversationsApi>();
        AddSecured<IMessagesApi>();
        AddSecured<IAccountApi>();
        AddSecured<IPersonnelApi>();
        AddSecured<IPresenceApi>();

        // =========================
        // 🔌 SignalR
        // =========================
        builder.Services.AddSingleton<ChatHubService>(sp =>
        {
            var token = sp.GetRequiredService<TokenStore>().GetToken();
            return new ChatHubService(BaseUrl, token);
        });

        builder.Services.AddSingleton<CallHubService>();

        // =========================
        // 🧠 Services internes
        // =========================
        builder.Services.AddSingleton<RoleService>();
        builder.Services.AddSingleton<AudioRecorderService>();
        builder.Services.AddSingleton(AudioManager.Current);
        builder.Services.AddSingleton<AudioPlayerService>();

        // =========================
        // 📦 ViewModels
        // =========================
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<ForgotPasswordViewModel>();
        builder.Services.AddTransient<ResetPasswordViewModel>();
        builder.Services.AddTransient<VerifyCodeViewModel>();

        builder.Services.AddTransient<SyndicDashboardViewModel>();

        builder.Services.AddTransient<ResidencesListViewModel>();
        builder.Services.AddTransient<ResidenceCreateViewModel>();
        builder.Services.AddTransient<ResidenceDetailsViewModel>();
        builder.Services.AddTransient<ResidenceEditViewModel>();

        builder.Services.AddTransient<BatimentsListViewModel>();
        builder.Services.AddTransient<BatimentCreateViewModel>();
        builder.Services.AddTransient<BatimentDetailsViewModel>();
        builder.Services.AddTransient<BatimentEditViewModel>();

        builder.Services.AddTransient<LotsListViewModel>();
        builder.Services.AddTransient<LotDetailsViewModel>();
        builder.Services.AddTransient<LotEditViewModel>();
        builder.Services.AddTransient<LotCreateViewModel>();

        builder.Services.AddTransient<AffectationsListViewModel>();
        builder.Services.AddTransient<AffectationCreateViewModel>();
        builder.Services.AddTransient<AffectationDetailsViewModel>();
        builder.Services.AddTransient<AffectationHistoriqueViewModel>();
        builder.Services.AddTransient<AffectationDashboardViewModel>();
        builder.Services.AddTransient<AffectationAnalyticsViewModel>();
        builder.Services.AddTransient<AffectationMaintenanceDashboardViewModel>();
        builder.Services.AddTransient<AffectationUserDashboardViewModel>();

        builder.Services.AddTransient<ChargesListViewModel>();
        builder.Services.AddTransient<ChargeCreateViewModel>();
        builder.Services.AddTransient<ChargeEditViewModel>();
        builder.Services.AddTransient<ChargeDetailsViewModel>();

        builder.Services.AddTransient<AppelsListViewModel>();
        builder.Services.AddTransient<AppelCreateViewModel>();
        builder.Services.AddTransient<AppelDetailsViewModel>();
        builder.Services.AddTransient<AppelEditViewModel>();

        builder.Services.AddTransient<PaiementsListViewModel>();
        builder.Services.AddTransient<PaiementCreateViewModel>();
        builder.Services.AddTransient<PaiementDetailsViewModel>();

        builder.Services.AddTransient<IncidentsListViewModel>();
        builder.Services.AddTransient<IncidentCreateViewModel>();
        builder.Services.AddTransient<IncidentDetailsViewModel>();
        builder.Services.AddTransient<IncidentEditViewModel>();
        builder.Services.AddTransient<IncidentStatusViewModel>();

        builder.Services.AddTransient<DevisTravauxListViewModel>();
        builder.Services.AddTransient<DevisTravauxDetailsViewModel>();
        builder.Services.AddTransient<DevisTravauxDecisionViewModel>();
        builder.Services.AddTransient<DevisTravauxCreateViewModel>();

        builder.Services.AddTransient<InterventionsListViewModel>();
        builder.Services.AddTransient<InterventionDetailsViewModel>();

        builder.Services.AddTransient<PrestatairesListViewModel>();
        builder.Services.AddTransient<PrestataireCreateViewModel>();
        builder.Services.AddTransient<PrestataireDetailsViewModel>();

        builder.Services.AddTransient<ChatViewModel>();
        builder.Services.AddTransient<ConversationsListViewModel>();
        builder.Services.AddTransient<NewConversationViewModel>();

        builder.Services.AddTransient<ActiveCallViewModel>();
        builder.Services.AddTransient<IncomingCallViewModel>();
        builder.Services.AddTransient<PresenceViewModel>();
        builder.Services.AddTransient<EmployesViewModel>();
        builder.Services.AddTransient<PlanningPresenceViewModel>();
        builder.Services.AddTransient<EmployeDetailsViewModel>();


        // =========================
        // 📄 Pages
        // =========================
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<ForgotPasswordPage>();
        builder.Services.AddTransient<VerifyCodePage>();
        builder.Services.AddTransient<ResetPasswordPage>();

        builder.Services.AddTransient<SyndicDashboardPage>();
        builder.Services.AddTransient<DrawerPage>();

        builder.Services.AddTransient<ResidencesPage>();
        builder.Services.AddTransient<ResidenceCreatePage>();
        builder.Services.AddTransient<ResidenceDetailsPage>();
        builder.Services.AddTransient<ResidenceEditPage>();

        builder.Services.AddTransient<BatimentsPage>();
        builder.Services.AddTransient<BatimentCreatePage>();
        builder.Services.AddTransient<BatimentDetailsPage>();
        builder.Services.AddTransient<BatimentEditPage>();

        builder.Services.AddTransient<LotsPage>();
        builder.Services.AddTransient<LotCreatePage>();
        builder.Services.AddTransient<LotDetailsPage>();
        builder.Services.AddTransient<LotEditPage>();

        builder.Services.AddTransient<AffectationsPage>();
        builder.Services.AddTransient<AffectationCreatePage>();
        builder.Services.AddTransient<AffectationDetailsPage>();
        builder.Services.AddTransient<AffectationHistoriquePage>();
        builder.Services.AddTransient<AffectationDashboardPage>();
        builder.Services.AddTransient<AffectationUserDashboardPage>();
        builder.Services.AddTransient<AffectationAnalyticsPage>();
        builder.Services.AddTransient<AffectationMaintenanceDashboardPage>();

        builder.Services.AddTransient<ChargesPage>();
        builder.Services.AddTransient<ChargeCreatePage>();
        builder.Services.AddTransient<ChargeEditPage>();
        builder.Services.AddTransient<ChargeDetailsPage>();

        builder.Services.AddTransient<AppelsPage>();
        builder.Services.AddTransient<AppelCreatePage>();
        builder.Services.AddTransient<AppelDetailsPage>();
        builder.Services.AddTransient<AppelEditPage>();

        builder.Services.AddTransient<PaiementsPage>();
        builder.Services.AddTransient<PaiementCreatePage>();
        builder.Services.AddTransient<PaiementDetailsPage>();

        builder.Services.AddTransient<IncidentsPage>();
        builder.Services.AddTransient<IncidentCreatePage>();
        builder.Services.AddTransient<IncidentDetailsPage>();
        builder.Services.AddTransient<IncidentEditPage>();
        builder.Services.AddTransient<IncidentStatusPage>();

        builder.Services.AddTransient<DevisTravauxPage>();
        builder.Services.AddTransient<DevisTravauxDetailsPage>();
        builder.Services.AddTransient<DevisTravauxDecisionPage>();
        builder.Services.AddTransient<DevisTravauxCreatePage>();

        builder.Services.AddTransient<InterventionsPage>();
        builder.Services.AddTransient<InterventionDetailsPage>();

        builder.Services.AddTransient<PrestatairesPage>();
        builder.Services.AddTransient<PrestataireCreatePage>();
        builder.Services.AddTransient<PrestataireDetailsPage>();

        builder.Services.AddTransient<ChatPage>();
        builder.Services.AddTransient<ConversationsPage>();
        builder.Services.AddTransient<NewConversationPage>();

        builder.Services.AddTransient<ActiveCallPage>();
        builder.Services.AddTransient<IncomingCallPage>();


        builder.Services.AddTransient<EmployesPage>();
        builder.Services.AddTransient<PlanningPresencePage>();
        builder.Services.AddTransient<PresencePage>();
        builder.Services.AddTransient<EmployeDetailsPage>();

        var app = builder.Build();
        ServiceHelper.Services = app.Services;
        return app;
    }
}
