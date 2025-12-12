using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Communication;
using SyndicApp.Application.Interfaces.Communication;
using SyndicApp.Infrastructure.Identity.Extensions;

namespace SyndicApp.API.Controllers.Communication
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }


        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var userId = User.GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized("UserId missing from token");

            var users = await _chatService.GetAllUsersExceptAsync(userId);
            return Ok(users);
        }

        [HttpPost("open")]
        public async Task<IActionResult> OpenChat([FromBody] OpenChatRequest req)
        {
            var currentUserId = User.GetUserId();
            if (currentUserId == Guid.Empty)
                return Unauthorized("UserId missing from token");

            var convId = await _chatService.OpenOrCreateConversationAsync(currentUserId, req.OtherUserId);
            return Ok(new OpenChatResponse { ConversationId = convId });
        }

    }
}
