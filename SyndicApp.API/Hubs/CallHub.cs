using Microsoft.AspNetCore.SignalR;
using SyndicApp.Application.Interfaces.AppelVocal;

namespace SyndicApp.API.Hubs
{
    public class CallHub : Hub
    {
        private readonly ICallService _callService;

        public CallHub(ICallService callService)
        {
            _callService = callService;
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"[SignalR] Connected UserId = {Context.UserIdentifier}");
            await base.OnConnectedAsync();
        }
        public async Task CallUser(Guid receiverId)
        {
            var callerId = Guid.Parse(Context.UserIdentifier!);
            await _callService.StartCallAsync(callerId, receiverId);

            await Clients.User(receiverId.ToString())
                .SendAsync("IncomingCall", callerId);
        }

        public async Task AcceptCall(Guid callId)
        {
            await _callService.AcceptCallAsync(callId);
        }

        public async Task EndCall(Guid callId)
        {
            await _callService.EndCallAsync(callId);
        }
    }
}
