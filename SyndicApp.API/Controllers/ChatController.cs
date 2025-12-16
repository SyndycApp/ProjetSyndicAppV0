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
        private readonly IImageMessageService _imageService;
        private readonly IDocumentMessageService _documentService;
        private readonly ILocationMessageService _locationService;

        public ChatController(
            IChatService chatService,
            IMessageService messageService,
            IImageMessageService imageService,
            IDocumentMessageService documentService,
            ILocationMessageService locationService)
        {
            _chatService = chatService;
            _messageService = messageService;
            _imageService = imageService;
            _documentService = documentService;
            _locationService = locationService;
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

        [HttpPost("message/image/{conversationId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SendImage(
      Guid conversationId,
      [FromForm] SendImageRequest request)
        {
            var userId = User.GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized();

            if (request.Image == null || request.Image.Length == 0)
                return BadRequest("Image is required");

            var message = await _imageService.SendImageAsync(
                userId,
                conversationId,
                request.Image.OpenReadStream(),
                request.Image.FileName,
                request.Image.ContentType
            );

            return Ok(message);
        }

        [HttpPost("message/document/{conversationId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SendDocument(
    Guid conversationId,
    [FromForm] SendDocumentRequest request)
        {
            var userId = User.GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized();

            if (request.Document == null || request.Document.Length == 0)
                return BadRequest("Document is required");

            var message = await _documentService.SendDocumentAsync(
                userId,
                conversationId,
                request.Document.OpenReadStream(),
                request.Document.FileName,
                request.Document.ContentType
            );

            return Ok(message);
        }


        [HttpPost("message/location")]
        public async Task<IActionResult> SendLocation(
           [FromBody] SendLocationRequest request)
        {
            var userId = User.GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized();

            var message = await _locationService.SendLocationAsync(
                userId,
                request.ConversationId,
                request.Latitude,
                request.Longitude
            );

            return Ok(message);
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
        // 🎤 ENVOI MESSAGE AUDIO (FIX SWAGGER)
        // =====================================================
        [HttpPost("message/audio/{conversationId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SendAudioMessage(
            Guid conversationId,
            [FromForm] SendAudioFormRequest request
        )
        {
            var userId = User.GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized();

            if (request.AudioFile == null || request.AudioFile.Length == 0)
                return BadRequest("Audio is required");

            var message = await _messageService.SendAudioMessageAsync(
                userId,
                conversationId,
                request.AudioFile.OpenReadStream(),
                request.AudioFile.FileName,
                request.AudioFile.ContentType
            );

            return Ok(message);
        }


        [HttpPost("message/{messageId}/reaction")]
        public async Task<IActionResult> React(Guid messageId,[FromBody] string emoji)
        {
            var userId = User.GetUserId();
            await _messageService.AddReactionAsync(messageId, userId, emoji);
            return Ok();
        }

    }
}
