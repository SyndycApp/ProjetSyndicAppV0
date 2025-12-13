using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui.ApplicationModel;
using System;

namespace SyndicApp.Mobile;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges =
        ConfigChanges.ScreenSize |
        ConfigChanges.Orientation |
        ConfigChanges.UiMode |
        ConfigChanges.ScreenLayout |
        ConfigChanges.SmallestScreenSize |
        ConfigChanges.Density)]

// Deep link: syndicapp://app/resetpassword?token=...&email=...
[IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = "syndicapp",
    DataHost = "app",
    DataPathPrefix = "/resetpassword")]
public class MainActivity : MauiAppCompatActivity
{
    private const int RecordAudioRequestCode = 1001;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // 🔐 Permission micro (OBLIGATOIRE pour l’audio)
        RequestAudioPermissionIfNeeded();

        // 🔗 Deep link
        HandleDeepLink(Intent);
    }

    // 🔧 Important : Intent? pour matcher la signature base
    protected override void OnNewIntent(Intent? intent)
    {
        base.OnNewIntent(intent);
        HandleDeepLink(intent);
    }

    // =========================
    // 🎤 PERMISSION MICRO
    // =========================
    private void RequestAudioPermissionIfNeeded()
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.M)
            return;

        if (CheckSelfPermission(Manifest.Permission.RecordAudio) != Permission.Granted)
        {
            RequestPermissions(
                new[] { Manifest.Permission.RecordAudio },
                RecordAudioRequestCode);
        }
    }

    // (Optionnel mais propre)
    public override void OnRequestPermissionsResult(
        int requestCode,
        string[] permissions,
        Permission[] grantResults)
    {
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        if (requestCode == RecordAudioRequestCode)
        {
            if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
            {
                System.Diagnostics.Debug.WriteLine("🎤 Permission micro accordée");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("❌ Permission micro refusée");
            }
        }
    }

    // =========================
    // 🔗 DEEP LINK RESET PASSWORD
    // =========================
    private void HandleDeepLink(Intent? intent)
    {
        var dataString = intent?.Data?.ToString();
        if (string.IsNullOrWhiteSpace(dataString))
            return;

        var uri = new Uri(dataString);

        if (uri.Scheme == "syndicapp" &&
            uri.Host == "app" &&
            uri.AbsolutePath == "/resetpassword")
        {
            var token = GetQueryParam(uri, "token");
            var email = GetQueryParam(uri, "email");

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync(
                    $"/resetpassword" +
                    $"?token={Uri.EscapeDataString(token ?? string.Empty)}" +
                    $"&email={Uri.EscapeDataString(email ?? string.Empty)}");
            });
        }
    }

    private static string? GetQueryParam(Uri uri, string key)
    {
        var q = uri.Query; // ?a=1&b=2
        if (string.IsNullOrEmpty(q) || q.Length < 2)
            return null;

        foreach (var p in q[1..].Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var kv = p.Split('=', 2);
            if (kv.Length == 2 &&
                string.Equals(kv[0], key, StringComparison.OrdinalIgnoreCase))
            {
                return Uri.UnescapeDataString(kv[1]);
            }
        }

        return null;
    }
}
