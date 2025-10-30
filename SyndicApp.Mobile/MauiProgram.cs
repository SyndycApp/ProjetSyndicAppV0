using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();


        const string BaseUrl = "http://192.168.11.144:5041"; 

        builder.Services.AddSingleton<TokenStore>();
        builder.Services.AddTransient<AuthHeaderHandler>();

        // Policy de résilience (timeouts, retry basique)
        var refitSettings = new RefitSettings();

        // Auth (pas de handler sur login/register)
        builder.Services.AddRefitClient<IAuthApi>(refitSettings)
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(BaseUrl));

        // Helper pour les clients sécurisés
        void AddSecured<T>() where T : class =>
            builder.Services.AddRefitClient<T>(refitSettings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(BaseUrl))
                .AddHttpMessageHandler<AuthHeaderHandler>();

        AddSecured<IPasswordApi>();
        AddSecured<IResidencesApi>();
        AddSecured<ILotsApi>();
        AddSecured<IAffectationsLotsApi>();
        AddSecured<IIncidentsApi>();
        AddSecured<IInterventionsApi>();
        AddSecured<IChargesApi>();
        AddSecured<IAppelsApi>();
        AddSecured<IPaiementsApi>();
        AddSecured<ISoldesApi>();
        AddSecured<IBatimentsApi>();
        AddSecured<IDevisTravauxApi>();
        AddSecured<ILocatairesTemporairesApi>();

        builder.Services.AddTransient<SyndicApp.Mobile.ViewModels.Auth.LoginViewModel>();
        builder.Services.AddTransient<SyndicApp.Mobile.ViewModels.Auth.PasswordViewModel>();

        builder.Services.AddTransient<SyndicApp.Mobile.ViewModels.Residences.ResidencesListViewModel>();
        builder.Services.AddTransient<SyndicApp.Mobile.ViewModels.Residences.ResidenceDetailsViewModel>();

        builder.Services.AddTransient<SyndicApp.Mobile.ViewModels.Incidents.IncidentsListViewModel>();
        builder.Services.AddTransient<SyndicApp.Mobile.ViewModels.Incidents.IncidentEditViewModel>();

        builder.Services.AddTransient<SyndicApp.Mobile.ViewModels.Interventions.InterventionsListViewModel>();
        builder.Services.AddTransient<SyndicApp.Mobile.ViewModels.Interventions.InterventionActionsViewModel>();

        builder.Services.AddTransient<SyndicApp.Mobile.ViewModels.Finances.ChargesListViewModel>();
        builder.Services.AddTransient<SyndicApp.Mobile.ViewModels.Finances.AppelsListViewModel>();
        builder.Services.AddTransient<SyndicApp.Mobile.ViewModels.Finances.PaiementsListViewModel>();
        builder.Services.AddTransient<SyndicApp.Mobile.ViewModels.Finances.SoldesViewModel>();

        builder.Services.AddTransient<SyndicApp.Mobile.ViewModels.Lots.LotsListViewModel>();
        builder.Services.AddTransient<SyndicApp.Mobile.ViewModels.Batiments.BatimentsListViewModel>();
        builder.Services.AddTransient<SyndicApp.Mobile.ViewModels.Affectations.AffectationsListViewModel>();
        builder.Services.AddTransient<SyndicApp.Mobile.ViewModels.LocatairesTemp.LocatairesTempListViewModel>();
        builder.Services.AddTransient<SyndicApp.Mobile.ViewModels.Travaux.DevisListViewModel>();

        var app = builder.Build();

        ServiceHelper.Services = app.Services;

        return app;
    }
}
