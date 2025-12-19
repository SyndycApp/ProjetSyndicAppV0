using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Communication;
using SyndicApp.Application.Interfaces.Communication;
using SyndicApp.Domain.Entities.Communication;
using SyndicApp.Infrastructure.Identity;


namespace SyndicApp.Infrastructure.Services.Communication
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAudioStorage _audioStorage;

        public MessageService(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IAudioStorage audioStorage)
        {
            _db = db;
            _userManager = userManager;
            _audioStorage = audioStorage;
        }

        // =========================
        // 👍 REACTION
        // =========================
        public async Task AddReactionAsync(Guid messageId, Guid userId, string emoji)
        {
            var message = await _db.Messages
                .FirstOrDefaultAsync(m => m.Id == messageId);

            if (message == null)
                throw new Exception("Message introuvable");

            var existing = await _db.MessageReactions
                .FirstOrDefaultAsync(r =>
                    r.MessageId == messageId &&
                    r.UserId == userId);

            if (existing != null)
            {
                if (existing.Emoji == emoji)
                    return;
                existing.Emoji = emoji;
            }
            else
            {
                _db.MessageReactions.Add(new MessageReaction
                {
                    MessageId = messageId,
                    UserId = userId,
                    Emoji = emoji
                });
            }

            await _db.SaveChangesAsync();
        }

        // =========================
        // 🔁 MAPPING COMMUN
        // =========================
        private MessageDto MapMessage(
            Message m,
            Dictionary<Guid, string> usersById)
        {
            usersById.TryGetValue(m.UserId, out var senderName);

            MessageDto? replyDto = null;

            if (m.ReplyToMessage != null)
            {
                usersById.TryGetValue(
                    m.ReplyToMessage.UserId,
                    out var replySenderName);

                replyDto = new MessageDto
                {
                    Id = m.ReplyToMessage.Id,
                    Contenu = m.ReplyToMessage.Contenu,
                    UserId = m.ReplyToMessage.UserId,
                    CreatedAt = m.ReplyToMessage.CreatedAt,
                    Type = m.ReplyToMessage.Type,
                    NomExpediteur = replySenderName ?? "Utilisateur"
                };
            }

            return new MessageDto
            {
                Id = m.Id,
                ConversationId = m.ConversationId,
                UserId = m.UserId,

                Contenu = m.Contenu,
                AudioUrl = m.AudioPath,

                FileUrl = m.FileUrl,
                FileName = m.FileName,
                ContentType = m.ContentType,

                Latitude = m.Latitude,
                Longitude = m.Longitude,

                Type = m.Type,
                CreatedAt = m.CreatedAt,
                IsRead = m.IsRead,
                ReadAt = m.ReadAt,

                ReplyToMessage = replyDto,

                Reactions = m.Reactions
          .Select(r => new MessageReactionDto
          {
              UserId = r.UserId,
              Emoji = r.Emoji
          })
          .ToList(),

                NomExpediteur = senderName ?? "Utilisateur"
            };

        }

        // =========================
        // 📩 GET MESSAGES
        // =========================
        public async Task<List<MessageDto>> GetMessagesAsync(Guid conversationId, Guid userId)
        {
            var messages = await _db.Messages
                .Include(m => m.ReplyToMessage)
                .Include(m => m.Reactions)
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();

            var usersById = await _userManager.Users
                .ToDictionaryAsync(u => u.Id, u => u.FullName);

            return messages
                .Select(m => MapMessage(m, usersById))
                .ToList();
        }

        // =========================
        // 📄 GET MESSAGES PAGINÉS
        // =========================
        public async Task<PagedMessagesDto> GetMessagesPaged(
            Guid conversationId,
            int page,
            int pageSize)
        {
            var query = _db.Messages
                .Include(m => m.ReplyToMessage)
                .Include(m => m.Reactions)
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.CreatedAt);

            var total = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var usersById = await _userManager.Users
                .ToDictionaryAsync(u => u.Id, u => u.FullName);

            return new PagedMessagesDto
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                Messages = items
                    .Select(m => MapMessage(m, usersById))
                    .ToList()
            };
        }

        // =========================
        // 👁️ MARK AS READ
        // =========================
        public async Task MarkMessagesAsReadAsync(Guid conversationId, Guid userId)
        {
            var messages = await _db.Messages
                .Where(m =>
                    m.ConversationId == conversationId &&
                    m.UserId != userId &&
                    !m.IsRead)
                .ToListAsync();

            foreach (var m in messages)
            {
                m.IsRead = true;
                m.ReadAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();
        }

        // =========================
        // ✉️ SEND TEXT
        // =========================
        public async Task<MessageDto> SendMessageAsync(
            Guid userId,
            SendMessageRequest request)
        {
            var message = new Message
            {
                ConversationId = request.ConversationId,
                UserId = userId,
                Contenu = request.Contenu,
                Type = MessageType.Text,
                ReplyToMessageId = request.ReplyToMessageId
            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            var usersById = await _userManager.Users
                .ToDictionaryAsync(u => u.Id, u => u.FullName);

            return MapMessage(message, usersById);
        }

        // =========================
        // 🎤 SEND AUDIO
        // =========================
        public async Task<MessageDto> SendAudioMessageAsync(
            Guid userId,
            Guid conversationId,
            Stream audioStream,
            string fileName,
            string contentType)
        {
            var audioPath = await _audioStorage.SaveAsync(
                audioStream,
                fileName,
                contentType);

            var message = new Message
            {
                ConversationId = conversationId,
                UserId = userId,
                AudioPath = audioPath,
                Type = MessageType.Audio
            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            var usersById = await _userManager.Users
                .ToDictionaryAsync(u => u.Id, u => u.FullName);

            return MapMessage(message, usersById);
        }
    }
}
