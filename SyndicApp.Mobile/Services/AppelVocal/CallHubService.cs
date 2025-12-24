using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Dispatching;

namespace SyndicApp.Mobile.Services.AppelVocal;

public class CallHubService
{
    private HubConnection? _connection;

    public event Action<Guid, Guid>? IncomingCall;
    public event Action<Guid>? CallAccepted;
    public event Action<Guid>? CallEnded;

    public async Task ConnectAsync(string baseUrl, string token)
    {
        if (_connection != null)
            return;

        Console.WriteLine("🔌 Connexion CallHub...");

        _connection = new HubConnectionBuilder()
            .WithUrl($"{baseUrl}/hubs/call", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(token);
            })
            .WithAutomaticReconnect()
            .Build();

        // ✅ HANDLERS APRÈS Build
        _connection.On<dynamic>("IncomingCall", data =>
        {
            var callId = Guid.Parse(data.callId.ToString());
            var callerId = Guid.Parse(data.callerId.ToString());

            Console.WriteLine($"📞 IncomingCall reçu → {callId}");

            MainThread.BeginInvokeOnMainThread(() =>
            {
                IncomingCall?.Invoke(callId, callerId);
            });
        });

        _connection.On<Guid>("CallAccepted", callId =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
                CallAccepted?.Invoke(callId));
        });

        _connection.On<Guid>("CallEnded", callId =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
                CallEnded?.Invoke(callId));
        });

        await _connection.StartAsync();
        Console.WriteLine("✅ CallHub connecté");
    }

    public Task AcceptCall(Guid callId)
        => _connection!.InvokeAsync("AcceptCall", callId);

    public Task EndCall(Guid callId)
        => _connection!.InvokeAsync("EndCall", callId);
}
