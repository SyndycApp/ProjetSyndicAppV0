using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Handlers;
using SyndicApp.Mobile.Services;
using System.Text.Json;

namespace SyndicApp.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // Choisis la bonne URL :
        // - Emulateur Android (sans adb reverse) : "http://10.0.2.2:5041"
        // - Appareil Android + adb reverse 5041:5041 : "http://127.0.0.1:5041"
        // - Téléphone réel sur même Wi-Fi : "http://<IP_PC>:5041"
        //const string BaseUrl = http:127.0.0.1:5041; //pastaepizza
        const string BaseUrl = "http://192.168.0.104:5041"; //home

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
                   c.Timeout = TimeSpan.FromSeconds(15);
               });

        // Forgot/reset password → généralement PUBLIC (pour lever ta régression)
        builder.Services.AddRefitClient<IPasswordApi>(refitSettings)
               .ConfigureHttpClient(c =>
               {
                   c.BaseAddress = new Uri(BaseUrl);
                   c.Timeout = TimeSpan.FromSeconds(15);
               });

        // Helper protégés (Bearer auto via AuthHeaderHandler)
        void AddSecured<T>() where T : class =>
            builder.Services.AddRefitClient<T>(refitSettings)
                   .ConfigureHttpClient(c =>
                   {
                       c.BaseAddress = new Uri(BaseUrl);
                       c.Timeout = TimeSpan.FromSeconds(15);
                   })
                   .AddHttpMessageHandler<AuthHeaderHandler>();

        AddSecured<IAccountApi>();   // /me, /logout protégés

        // VMs
        builder.Services.AddTransient<ViewModels.Auth.LoginViewModel>();
        builder.Services.AddTransient<ViewModels.Auth.RegisterViewModel>();
        builder.Services.AddTransient<ViewModels.Auth.ForgotPasswordViewModel>();
        builder.Services.AddTransient<ViewModels.Dashboard.SyndicDashboardViewModel>();

        // Pages
        builder.Services.AddTransient<Views.Auth.LoginPage>();
        builder.Services.AddTransient<Views.Auth.RegisterPage>();
        builder.Services.AddTransient<Views.Auth.ForgotPasswordPage>();
        builder.Services.AddTransient<Views.Dashboard.SyndicDashboardPage>();

        var app = builder.Build();
        ServiceHelper.Services = app.Services;
        return app;
    }
}
