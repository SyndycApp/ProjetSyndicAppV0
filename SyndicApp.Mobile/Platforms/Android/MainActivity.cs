using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System;

namespace SyndicApp.Mobile;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]

// Deep link: syndicapp://app/resetpassword?token=...&email=...
[IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = "syndicapp",
    DataHost = "app",
    DataPathPrefix = "/resetpassword")]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        HandleDeepLink(Intent);
    }

    // 🔧 IMPORTANT : Intent? pour matcher la signature base (corrige CS8765)
    protected override void OnNewIntent(Intent? intent)
    {
        base.OnNewIntent(intent);
        HandleDeepLink(intent);
    }

    private void HandleDeepLink(Intent? intent)
    {
        // On récupère la string de l’URI de façon sûre (corrige CS8604)
        var dataString = intent?.Data?.ToString();
        if (string.IsNullOrWhiteSpace(dataString))
            return;

        var uri = new Uri(dataString);

        if (uri.Scheme == "syndicapp" && uri.Host == "app" && uri.AbsolutePath == "/resetpassword")
        {
            var token = GetQueryParam(uri, "token");
            var email = GetQueryParam(uri, "email");

            Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync(
                    $"/resetpassword?token={Uri.EscapeDataString(token ?? "")}&email={Uri.EscapeDataString(email ?? "")}");
            });
        }
    }

    private static string? GetQueryParam(Uri uri, string key)
    {
        var q = uri.Query; // ?a=1&b=2
        if (string.IsNullOrEmpty(q) || q.Length < 2) return null;

        foreach (var p in q[1..].Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var kv = p.Split('=', 2);
            if (kv.Length == 2 && string.Equals(kv[0], key, StringComparison.OrdinalIgnoreCase))
                return Uri.UnescapeDataString(kv[1]);
        }

        return null;
    }
}
