using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SyndicApp.API.Extensions;
using SyndicApp.API.Hubs;
using SyndicApp.Application.DTOs.AppelVocal;
using SyndicApp.Application.Interfaces.AppelVocal;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/calls")]
    [Authorize]
    public class CallsController : ControllerBase
    {
        private readonly ICallService _callService;
        private readonly IHubContext<CallHub> _hubContext;

        public CallsController(
            ICallService callService,
            IHubContext<CallHub> hubContext)
        {
            _callService = callService;
            _hubContext = hubContext;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartCall([FromBody] CreateCallDto dto)
        {
            var callerId = User.GetUserId();
            var call = await _callService.StartCallAsync(callerId, dto.ReceiverId);
            return Ok(call);
        }

        [HttpPost("{callId}/end")]
        public async Task<IActionResult> End(Guid callId)
        {
            await _callService.EndCallAsync(callId);
            return Ok();
        }

        [HttpGet("history")]
        public async Task<IActionResult> History()
        {
            return Ok(await _callService.GetHistoryAsync(User.GetUserId()));
        }

        [HttpGet("missed")]
        public async Task<IActionResult> Missed()
        {
            return Ok(await _callService.GetMissedCallsAsync(User.GetUserId()));
        }

        // 🧪 DEV UNIQUEMENT (Swagger)
        [HttpPost("{callId}/accept-dev")]
        public async Task<IActionResult> AcceptDev(Guid callId)
        {
            await _callService.AcceptCallAsync(callId);

            var call = await _callService.GetByIdAsync(callId);
            if (call == null) return NotFound();

            await _hubContext.Clients.User(call.CallerId.ToString())
                .SendAsync("CallAccepted", callId);

            await _hubContext.Clients.User(call.ReceiverId.ToString())
                .SendAsync("CallAccepted", callId);

            return Ok();
        }
    }
}
