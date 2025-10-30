using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly TokenStore _tokens;
    public AuthHeaderHandler(TokenStore tokens) => _tokens = tokens;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var token = await _tokens.GetAsync();
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, ct);
    }
}
