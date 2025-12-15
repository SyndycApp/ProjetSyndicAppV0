using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Communication;
using SyndicApp.Application.Interfaces.Communication;
using SyndicApp.Domain.Entities.Communication;
using SyndicApp.Domain.Enums;
using SyndicApp.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        // 🔁 MAPPING COMMUN
        // =========================
        private MessageDto MapMessage(Message m, string? nomExpediteur = null)
        {
            return new MessageDto
            {
                Id = m.Id,
                ConversationId = m.ConversationId,
                UserId = m.UserId,

                // 📝 TEXTE
                Contenu = m.Contenu,

                // 🔊 AUDIO 
                AudioUrl = m.AudioPath,

                // 📎 IMAGE / DOCUMENT
                FileUrl = m.FileUrl,
                FileName = m.FileName,
                ContentType = m.ContentType,

                // 📍 LOCALISATION
                Latitude = m.Latitude,
                Longitude = m.Longitude,

                // META
                Type = m.Type,
                CreatedAt = m.CreatedAt,
                IsRead = m.IsRead,
                ReadAt = m.ReadAt,
                NomExpediteur = nomExpediteur ?? "Utilisateur"
            };
        }

        // =========================
        // 📩 GET MESSAGES (LISTE)
        // =========================
        public async Task<List<MessageDto>> GetMessagesAsync(Guid conversationId, Guid userId)
        {
            var messages = await _db.Messages
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();

            var users = await _userManager.Users.ToListAsync();

            return messages.Select(m =>
            {
                var userName = users.FirstOrDefault(u => u.Id == m.UserId)?.FullName;
                return MapMessage(m, userName);
            }).ToList();
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
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.CreatedAt);

            var total = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedMessagesDto
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                Messages = items.Select(m => MapMessage(m)).ToList()
            };
        }

        // =========================
        // 👁️ MARQUER COMME LU
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
        // ✉️ ENVOI MESSAGE TEXTE
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
                IsRead = false,
                ReadAt = null
            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(userId.ToString());

            return MapMessage(message, user?.FullName);
        }

        // =========================
        // 🎤 ENVOI MESSAGE AUDIO
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
                Type = MessageType.Audio,
                IsRead = false,
                ReadAt = null
            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(userId.ToString());

            return MapMessage(message, user?.FullName);
        }
    }
}
