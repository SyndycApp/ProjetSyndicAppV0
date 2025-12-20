using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.SignalR.Client;

namespace SyndicApp.Mobile.ViewModels.AppelVocal
{
    public partial class IncomingCallViewModel : ObservableObject
    {
        private HubConnection? _connection;

        [ObservableProperty]
        Guid callerId;

        [ObservableProperty]
        Guid callId;

        public async Task ConnectAsync(string token)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://IP_LOCAL:5041/hubs/call", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token)!;
                })
                .WithAutomaticReconnect()
                .Build();

            _connection.On<Guid>("IncomingCall", id =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    CallerId = id;
                });
            });

            await _connection.StartAsync();
        }

        public async Task AcceptAsync()
        {
            if (_connection != null)
                await _connection.InvokeAsync("AcceptCall", CallId);
        }

        [RelayCommand]
        private async Task Accept()
        {
            await _connection!.InvokeAsync("AcceptCall", CallId);

            await Shell.Current.GoToAsync("active-call", new Dictionary<string, object>
            {
                ["CallId"] = CallId,
                ["OtherUserId"] = CallerId
            });

        }

        public async Task RejectAsync()
        {
            if (_connection != null)
                await _connection.InvokeAsync("EndCall", CallId);
        }
    }
}
