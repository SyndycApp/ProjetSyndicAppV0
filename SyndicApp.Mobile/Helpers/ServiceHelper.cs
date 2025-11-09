// Helpers/ServiceHelper.cs
using Java.Security;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SyndicApp.Mobile;

public static class ServiceHelper
{
    public static IServiceProvider Services { get; set; } = default!;


    public static IServiceProvider Current =>
            Application.Current!.Handler!.MauiContext!.Services;

    public static T GetRequiredService<T>() where T : notnull
        => (Services ?? throw new InvalidOperationException("ServiceProvider not initialized"))
           .GetRequiredService<T>();


    public static T Get<T>() where T : notnull =>
    Current.GetRequiredService<T>();

    // Variante tolérante (null si non inscrit)
    public static T? TryGet<T>() where T : class =>
        Current.GetService<T>();
}
