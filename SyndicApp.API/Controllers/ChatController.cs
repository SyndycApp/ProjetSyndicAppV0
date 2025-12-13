using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Communication;
using SyndicApp.Application.Interfaces.Communication;
using SyndicApp.Infrastructure.Identity.Extensions;
using SyndicApp.API.Requests;

namespace SyndicApp.API.Controllers.Communication
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;

        public ChatController(
            IChatService chatService,
            IMessageService messageService)
        {
            _chatService = chatService;
            _messageService = messageService;
        }

        // =====================================================
        // 👤 USERS (EXISTANT - INCHANGÉ)
        // =====================================================
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var userId = User.GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized("UserId missing from token");

            var users = await _chatService.GetAllUsersExceptAsync(userId);
            return Ok(users);
        }

        // =====================================================
        // 💬 OPEN / CREATE CONVERSATION (EXISTANT - INCHANGÉ)
        // =====================================================
        [HttpPost("open")]
        public async Task<IActionResult> OpenChat([FromBody] OpenChatRequest req)
        {
            var currentUserId = User.GetUserId();
            if (currentUserId == Guid.Empty)
                return Unauthorized("UserId missing from token");

            var convId = await _chatService
                .OpenOrCreateConversationAsync(currentUserId, req.OtherUserId);

            return Ok(new OpenChatResponse
            {
                ConversationId = convId
            });
        }

        // =====================================================
        // 📩 GET MESSAGES (LISTE)
        // =====================================================
        [HttpGet("{conversationId}/messages")]
        public async Task<IActionResult> GetMessages(Guid conversationId)
        {
            var userId = User.GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized("UserId missing from token");

            var messages = await _messageService
                .GetMessagesAsync(conversationId, userId);

            return Ok(messages);
        }

        // =====================================================
        // 📄 GET MESSAGES PAGINÉS
        // =====================================================
        [HttpGet("{conversationId}/messages/paged")]
        public async Task<IActionResult> GetMessagesPaged(
            Guid conversationId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _messageService
                .GetMessagesPaged(conversationId, page, pageSize);

            return Ok(result);
        }

        // =====================================================
        // 👁️ MARQUER COMME LU
        // =====================================================
        [HttpPost("{conversationId}/read")]
        public async Task<IActionResult> MarkAsRead(Guid conversationId)
        {
            var userId = User.GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized("UserId missing from token");

            await _messageService
                .MarkMessagesAsReadAsync(conversationId, userId);

            return NoContent();
        }

        // =====================================================
        // ✉️ ENVOI MESSAGE TEXTE
        // =====================================================
        [HttpPost("message")]
        public async Task<IActionResult> SendTextMessage(
            [FromBody] SendMessageRequest request)
        {
            var userId = User.GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized("UserId missing from token");

            var message = await _messageService
                .SendMessageAsync(userId, request);

            return Ok(message);
        }

        // =====================================================
        // 🎤 ENVOI MESSAGE AUDIO
        // =====================================================
        [HttpPost("message/audio/{conversationId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SendAudioMessage(
    Guid conversationId,
    [FromForm] IFormFile AudioFile
)
        {
            var userId = User.GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized("UserId missing from token");

            if (AudioFile == null || AudioFile.Length == 0)
                return BadRequest("AudioFile is required");

            var message = await _messageService.SendAudioMessageAsync(
                userId,
                conversationId,
                AudioFile.OpenReadStream(),
                AudioFile.FileName,
                AudioFile.ContentType
            );

            return Ok(message);
        }

    }
}
