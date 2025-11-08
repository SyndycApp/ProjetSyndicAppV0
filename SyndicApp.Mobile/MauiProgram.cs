using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Converters;
using SyndicApp.Mobile.Handlers;
using SyndicApp.Mobile.Services;
using SyndicApp.Mobile.ViewModels.Auth;
using SyndicApp.Mobile.ViewModels.Finances;
using SyndicApp.Mobile.Views.Finances;
using SyndicApp.Mobile.Views;
using SyndicApp.Mobile.Views.Auth;
using SyndicApp.Mobile.Views.Dashboard;
using SyndicApp.Mobile.ViewModels.Auth;
using System.Text.Json;
using SyndicApp.Mobile.ViewModels.Dashboard;

namespace SyndicApp.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // Choisis la bonne URL :        
        const string BaseUrl = "http://192.168.0.104:5041";


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

        AddSecured<IAccountApi>();   // /me, /logout protégés
        AddSecured<IAppelsApi>();

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


        var app = builder.Build();
        ServiceHelper.Services = app.Services;
        return app;
    }
}
