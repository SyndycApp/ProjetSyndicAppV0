using Microsoft.AspNetCore.SignalR.Client;

namespace SyndicApp.Mobile.Services.AppelVocal;

public class CallHubService
{
    private HubConnection? _connection;

    public event Action<Guid>? CallAccepted;
    public event Action<Guid>? CallEnded;

    public async Task ConnectAsync(string baseUrl, string token)
    {
        if (_connection != null) return;

        _connection = new HubConnectionBuilder()
            .WithUrl($"{baseUrl}/hubs/call", o =>
                o.AccessTokenProvider = () => Task.FromResult(token)!)
            .WithAutomaticReconnect()
            .Build();

        _connection.On<Guid>("CallAccepted", callId =>
            MainThread.BeginInvokeOnMainThread(() =>
                CallAccepted?.Invoke(callId)));

        _connection.On<Guid>("CallEnded", callId =>
            MainThread.BeginInvokeOnMainThread(() =>
                CallEnded?.Invoke(callId)));

        await _connection.StartAsync();
    }

    public Task AcceptCall(Guid callId)
        => _connection!.InvokeAsync("AcceptCall", callId);

    public Task EndCall(Guid callId)
        => _connection!.InvokeAsync("EndCall", callId);
}
