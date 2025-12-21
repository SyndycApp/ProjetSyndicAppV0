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

        public async Task AcceptCall(Guid callId)
        {
            await _callService.AcceptCallAsync(callId);

            var call = await _callService.GetByIdAsync(callId);
            if (call == null) return;

            await Clients.User(call.CallerId.ToString())
                .SendAsync("CallAccepted", callId);

            await Clients.User(call.ReceiverId.ToString())
                .SendAsync("CallAccepted", callId);
        }
        public async Task SendOffer(Guid callId, Guid targetUserId, string sdp)
        {
            await Clients.User(targetUserId.ToString())
                .SendAsync("ReceiveOffer", callId, sdp);
        }

        public async Task SendAnswer(Guid callId, Guid targetUserId, string sdp)
        {
            await Clients.User(targetUserId.ToString())
                .SendAsync("ReceiveAnswer", callId, sdp);
        }

        public async Task SendIceCandidate(Guid callId, Guid targetUserId, string candidate)
        {
            await Clients.User(targetUserId.ToString())
                .SendAsync("ReceiveIceCandidate", callId, candidate);
        }

        public async Task EndCall(Guid callId)
        {
            await _callService.EndCallAsync(callId);
            await Clients.All.SendAsync("CallEnded", callId);
        }
    }
}
