using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Dispatching;

namespace SyndicApp.Mobile.Services.AppelVocal;

public class CallHubService
{
    private HubConnection? _connection;

    // ===== EVENTS =====
    public event Action<Guid, Guid>? IncomingCall;
    public event Action<Guid>? CallAccepted;
    public event Action<Guid>? CallEnded;

    public async Task ConnectAsync(string baseUrl, string token)
    {
        if (_connection != null)
            return;

        Console.WriteLine("🔌 Connexion CallHub...");
        Console.WriteLine($"🔑 Token (début) = {token[..20]}...");

        _connection.On("HubReady", () =>
        {
            Console.WriteLine("🟢 Hub prêt à recevoir des appels");
        });

        _connection = new HubConnectionBuilder()
            .WithUrl($"{baseUrl}/hubs/call", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(token);
            })
            .WithAutomaticReconnect()
            .Build();

        // =========================
        // 📞 APPEL ENTRANT
        // =========================
        _connection.On<dynamic>("IncomingCall", data =>
        {
            Guid callId = Guid.Parse(data.callId.ToString());
            Guid callerId = Guid.Parse(data.callerId.ToString());

            Console.WriteLine($"📞 IncomingCall SIGNALR reçu → {callId}");

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Console.WriteLine($"📞 Dispatch IncomingCall → {callId}");
                IncomingCall?.Invoke(callId, callerId);
            });
        });

        _connection.On<Guid>("CallAccepted", callId =>
        {
            Console.WriteLine($"✅ CallAccepted {callId}");
            MainThread.BeginInvokeOnMainThread(() =>
                CallAccepted?.Invoke(callId));
        });

        _connection.On<Guid>("CallEnded", callId =>
        {
            Console.WriteLine($"❌ CallEnded {callId}");
            MainThread.BeginInvokeOnMainThread(() =>
                CallEnded?.Invoke(callId));
        });

        await _connection.StartAsync();
        Console.WriteLine("🔌 CallHub connecté");
    }

    public Task AcceptCall(Guid callId)
        => _connection!.InvokeAsync("AcceptCall", callId);

    public Task EndCall(Guid callId)
        => _connection!.InvokeAsync("EndCall", callId);
}
