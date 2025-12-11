using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Communication;
using SyndicApp.Application.Interfaces.Communication;
using SyndicApp.Infrastructure.Identity.Extensions;


namespace SyndicApp.API.Controllers.Communication
{
    [ApiController]
    [Route("api/messages")]
    [Authorize] // protection JWT
    public class MessageController : ControllerBase
    {
        private readonly IConversationService _conversationService;
        private readonly IMessageService _messageService;

        public MessageController(
            IConversationService conversationService,
            IMessageService messageService)
        {
            _conversationService = conversationService;
            _messageService = messageService;
        }

        // ==============================
        // 1️⃣ LISTE DES CONVERSATIONS
        // ==============================
        [HttpGet("conversations")]
        public async Task<IActionResult> GetConversations()
        {
            var userId = User.GetUserId(); // extension Identity
            var result = await _conversationService.GetUserConversationsAsync(userId);
            return Ok(result);
        }

        // ==============================
        // 2️⃣ CRÉER UNE CONVERSATION
        // ==============================
        [HttpPost("conversations")]
        public async Task<IActionResult> CreateConversation([FromBody] CreateConversationRequest req)
        {
            var userId = User.GetUserId();
            var result = await _conversationService.CreateConversationAsync(userId, req);
            return Ok(result);
        }

        // ==============================
        // 3️⃣ LISTER MESSAGES D’UNE CONVERSATION
        // ==============================
        [HttpGet("{conversationId:guid}")]
        public async Task<IActionResult> GetMessages(Guid conversationId)
        {
            var userId = User.GetUserId();
            var result = await _messageService.GetMessagesAsync(conversationId, userId);
            return Ok(result);
        }


        [HttpGet("{conversationId:guid}/messages")]
        public async Task<IActionResult> GetMessages(Guid conversationId, int page = 1, int pageSize = 30)
        {
            var result = await _messageService.GetMessagesPaged(conversationId, page, pageSize);
            return Ok(result);
        }


        [HttpPost("{conversationId:guid}/read")]
        public async Task<IActionResult> MarkAsRead(Guid conversationId)
        {
            var userId = User.GetUserId();
            await _messageService.MarkMessagesAsReadAsync(conversationId, userId);
            return Ok();
        }


        // ==============================
        // 4️⃣ ENVOYER UN MESSAGE
        // ==============================
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest req)
        {
            var userId = User.GetUserId();
            var result = await _messageService.SendMessageAsync(userId, req);
            return Ok(result);
        }
    }
}
