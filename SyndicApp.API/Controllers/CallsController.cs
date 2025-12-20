using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.API.Extensions;
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

        public CallsController(ICallService callService)
        {
            _callService = callService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartCall([FromBody] CreateCallDto dto)
        {
            try
            {
                var callerId = User.GetUserId();

                if (callerId == Guid.Empty)
                    return Unauthorized("UserId invalide");

                var call = await _callService.StartCallAsync(callerId, dto.ReceiverId);
                return Ok(call);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
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
            var userId = User.GetUserId();
            return Ok(await _callService.GetHistoryAsync(userId));
        }

        [HttpGet("missed")]
        public async Task<IActionResult> Missed()
        {
            var userId = User.GetUserId();
            return Ok(await _callService.GetMissedCallsAsync(userId));
        }
        
    }
}
