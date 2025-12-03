using Microcharts.Maui;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Converters;
using SyndicApp.Mobile.Handlers;
using SyndicApp.Mobile.Services;
using SyndicApp.Mobile.ViewModels.Affectations;
using SyndicApp.Mobile.ViewModels.Auth;
using SyndicApp.Mobile.ViewModels.Batiments;
using SyndicApp.Mobile.ViewModels.Dashboard;
using SyndicApp.Mobile.ViewModels.Finances;
using SyndicApp.Mobile.ViewModels.Incidents;
using SyndicApp.Mobile.ViewModels.Lots;
using SyndicApp.Mobile.ViewModels.Personnel;
using SyndicApp.Mobile.ViewModels.Residences;
using SyndicApp.Mobile.Views;
using SyndicApp.Mobile.Views.Affectations;
using SyndicApp.Mobile.Views.Auth;
using SyndicApp.Mobile.Views.Batiments;
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
        builder.UseMauiApp<App>();
        builder.UseMicrocharts();

        // Choisis la bonne URL :        
        const string BaseUrl = "http://192.168.1.121:5041";


        // Refit JSON insensible à la casse
        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var refitSettings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(jsonOptions)
        };

        // Stores & handlers
        builder.Services.AddSingleton<TokenStore>();
        builder.Services.AddTransient<AuthHeaderHandler>();

        // --- Clients Refit ---
        // Public (pas de bearer)
        builder.Services.AddRefitClient<IAuthApi>(refitSettings)
               .ConfigureHttpClient(c =>
               {
                   c.BaseAddress = new Uri(BaseUrl);
                   c.Timeout = TimeSpan.FromSeconds(60);
               });

        builder.Services.AddRefitClient<IResidencesApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(BaseUrl))
    .AddHttpMessageHandler<AuthHeaderHandler>();

        // Forgot/reset password → généralement PUBLIC (pour lever ta régression)
        builder.Services.AddRefitClient<IPasswordApi>(refitSettings)
               .ConfigureHttpClient(c =>
               {
                   c.BaseAddress = new Uri(BaseUrl);
                   c.Timeout = TimeSpan.FromSeconds(60);
               });

        // Helper protégés (Bearer auto via AuthHeaderHandler)
        void AddSecured<T>() where T : class =>
            builder.Services.AddRefitClient<T>(refitSettings)
                   .ConfigureHttpClient(c =>
                   {
                       c.BaseAddress = new Uri(BaseUrl);
                       c.Timeout = TimeSpan.FromSeconds(60);
                   })
                   .AddHttpMessageHandler<AuthHeaderHandler>();

        builder.Services.AddRefitClient<IBatimentsApi>()
         .ConfigureHttpClient(c => c.BaseAddress = new Uri(BaseUrl))
         .AddHttpMessageHandler<AuthHeaderHandler>();

        builder.Services.AddRefitClient<IAppelsApi>(refitSettings)
                        .ConfigureHttpClient(c => c.BaseAddress = new Uri(BaseUrl))
                        .AddHttpMessageHandler<AuthHeaderHandler>();

        builder.Services
                .AddRefitClient<IDevisTravauxApi>()
                .AddHttpMessageHandler<AuthHeaderHandler>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(BaseUrl));

        builder.Services.AddRefitClient<IPrestatairesApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(BaseUrl))
            .AddHttpMessageHandler<AuthHeaderHandler>();

        AddSecured<IAccountApi>();   
        AddSecured<IAppelsApi>();
        AddSecured<IResidencesApi>();
        AddSecured<IBatimentsApi>();
        AddSecured<ILotsApi>();
        AddSecured<IUsersApi>();
        AddSecured<IAffectationsLotsApi>();
        AddSecured<IAffectationLotsApiAlt>();
        AddSecured<IChargesApi>();
        AddSecured<IPaiementsApi>();
        AddSecured<IIncidentsApi>();
        AddSecured<IDevisTravauxApi>();
        AddSecured<IInterventionsApi>();
        AddSecured<IPrestatairesApi>();

        // VMs
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<ForgotPasswordViewModel>();
        builder.Services.AddTransient<SyndicDashboardViewModel>();
        builder.Services.AddTransient<ResetPasswordViewModel>();
        builder.Services.AddTransient<VerifyCodeViewModel>();
        builder.Services.AddTransient<AppelsListViewModel>();
        builder.Services.AddTransient<AppelCreateViewModel>();
        builder.Services.AddTransient<AppelDetailsViewModel>();
        builder.Services.AddTransient<AppelEditViewModel>();
        builder.Services.AddTransient<ResidencesListViewModel>();
        builder.Services.AddTransient<ResidenceCreateViewModel>();
        builder.Services.AddTransient<ResidenceDetailsViewModel>();
        builder.Services.AddTransient<ResidenceEditViewModel>();
        builder.Services.AddTransient<BatimentsListViewModel>();
        builder.Services.AddTransient<BatimentCreateViewModel>();
        builder.Services.AddTransient<BatimentDetailsViewModel>();
        builder.Services.AddTransient<BatimentEditViewModel>();
        builder.Services.AddTransient<LotDetailsViewModel>();
        builder.Services.AddTransient<LotEditViewModel>();
        builder.Services.AddTransient<LotCreateViewModel>();
        builder.Services.AddTransient<LotsListViewModel>();
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


        // Converters (si DI utilisé)
        builder.Services.AddSingleton<ProgressConverter>(); builder.Services.AddTransient<ResetWithCodeViewModel>();


        // Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<ForgotPasswordPage>();
        builder.Services.AddTransient<SyndicDashboardPage>();        
        builder.Services.AddTransient<ResetPasswordPage>();
        builder.Services.AddTransient<VerifyCodePage>();
        builder.Services.AddTransient<ResetWithCodePage>();
        builder.Services.AddTransient<AppelsPage>();
        builder.Services.AddTransient<AppelCreatePage>();
        builder.Services.AddTransient<AppelDetailsPage>();
        builder.Services.AddTransient<AppelEditPage>();
        builder.Services.AddTransient<DrawerPage>();
        builder.Services.AddTransient<ResidencesPage>();
        builder.Services.AddTransient<ResidenceCreatePage>();
        builder.Services.AddTransient<ResidenceDetailsPage>();
        builder.Services.AddTransient<ResidenceEditPage>();
        builder.Services.AddTransient<BatimentsPage>();
        builder.Services.AddTransient<BatimentCreatePage>();
        builder.Services.AddTransient<BatimentDetailsPage>();
        builder.Services.AddTransient<BatimentEditPage>();
        builder.Services.AddTransient<LotDetailsPage>();
        builder.Services.AddTransient<LotEditPage>();
        builder.Services.AddTransient<LotCreatePage>();
        builder.Services.AddTransient<LotsPage>();
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


        var app = builder.Build();
        ServiceHelper.Services = app.Services;
        return app;
    }
}
