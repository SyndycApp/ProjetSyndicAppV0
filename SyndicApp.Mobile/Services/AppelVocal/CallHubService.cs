using Microsoft.AspNetCore.SignalR.Client;

namespace SyndicApp.Mobile.Services.AppelVocal;

public class CallHubService
{
    private HubConnection? _connection;

    public event Action<Guid, Guid>? IncomingCall;
    public event Action<Guid>? CallEnded;

    public async Task ConnectAsync(string baseUrl, string token)
    {
        if (_connection != null)
            return;

        _connection = new HubConnectionBuilder()
            .WithUrl($"{baseUrl}/hubs/call", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(token)!;
            })
            .WithAutomaticReconnect()
            .Build();

        _connection.On<Guid, Guid>("IncomingCall", (callId, callerId) =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
                IncomingCall?.Invoke(callId, callerId));
        });

        _connection.On<Guid>("CallEnded", callId =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
                CallEnded?.Invoke(callId));
        });

        await _connection.StartAsync();
    }

    public async Task AcceptCall(Guid callId)
        => await _connection!.InvokeAsync("AcceptCall", callId);

    public async Task EndCall(Guid callId)
        => await _connection!.InvokeAsync("EndCall", callId);
}
