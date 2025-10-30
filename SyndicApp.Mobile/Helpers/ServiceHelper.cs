// Helpers/ServiceHelper.cs
using System;
using Microsoft.Extensions.DependencyInjection;

namespace SyndicApp.Mobile;

public static class ServiceHelper
{
    public static IServiceProvider Services { get; set; } = default!;

    public static T GetRequiredService<T>() where T : notnull
        => Services.GetRequiredService<T>();
}
