// Handlers/AuthHeaderHandler.cs
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.Handlers;

public sealed class AuthHeaderHandler : DelegatingHandler
{
    private readonly SyndicApp.Mobile.Services.TokenStore _tokenStore; 

    public AuthHeaderHandler(SyndicApp.Mobile.Services.TokenStore tokenStore) 
        => _tokenStore = tokenStore;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var token = _tokenStore.GetToken();
        var has = !string.IsNullOrWhiteSpace(token);
        System.Diagnostics.Debug.WriteLine($"[AuthHeaderHandler] {request.Method} {request.RequestUri} | token? {has}");
        if (has)
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        return base.SendAsync(request, ct);
    }

}
