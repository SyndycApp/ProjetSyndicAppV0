// Handlers/AuthHeaderHandler.cs
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Dispatching; // MainThread
using SyndicApp.Mobile.Services;

namespace SyndicApp.Mobile.Handlers;

public sealed class AuthHeaderHandler : DelegatingHandler
{
    private readonly TokenStore _tokenStore;

    public AuthHeaderHandler(TokenStore tokenStore) => _tokenStore = tokenStore;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var token = _tokenStore.GetToken(); // ← sync
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, ct);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            // Auto logout soft
            _tokenStore.Clear();

            // Rebond vers login (fire & forget sur le thread UI)
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try { await Shell.Current.GoToAsync("//login"); } catch { /* ignore */ }
            });
        }

        return response;
    }
}
