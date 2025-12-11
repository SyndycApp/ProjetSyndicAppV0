using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Communication;
using SyndicApp.Application.Interfaces.Communication;
using SyndicApp.Domain.Entities.Communication;
using SyndicApp.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Services.Communication
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessageService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        private MessageDto MapMessage(Message m)
        {
            return new MessageDto
            {
                Id = m.Id,
                ConversationId = m.ConversationId,
                UserId = m.UserId,
                Contenu = m.Contenu,
                CreatedAt = m.CreatedAt,
                IsRead = m.IsRead,    
                ReadAt = m.ReadAt,
                NomExpediteur = "" 
            };
        }
        public async Task<List<MessageDto>> GetMessagesAsync(Guid conversationId, Guid userId)
        {
            var messages = await _db.Messages
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();

            var users = await _userManager.Users.ToListAsync();

            return messages.Select(m => new MessageDto
            {
                Id = m.Id,
                ConversationId = m.ConversationId,
                UserId = m.UserId,
                Contenu = m.Contenu,
                CreatedAt = m.CreatedAt,
                IsRead = m.IsRead,             
                ReadAt = m.ReadAt,
                NomExpediteur = users.FirstOrDefault(u => u.Id == m.UserId)?.FullName ?? "Utilisateur"
            }).ToList();
        }

        public async Task<PagedMessagesDto> GetMessagesPaged(Guid conversationId, int page, int pageSize)
        {
            var query = _db.Messages
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.CreatedAt);

            var total = await query.CountAsync();

            var items = await query.Skip((page - 1) * pageSize)
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

        public async Task MarkMessagesAsReadAsync(Guid conversationId, Guid userId)
        {
            var messages = await _db.Messages
                .Where(m => m.ConversationId == conversationId && m.UserId != userId && !m.IsRead)
                .ToListAsync();

            foreach (var m in messages)
            {
                m.IsRead = true;
                m.ReadAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();
        }

        public async Task<MessageDto> SendMessageAsync(Guid userId, SendMessageRequest request)
        {
            var message = new Message
            {
                ConversationId = request.ConversationId,
                UserId = userId,
                Contenu = request.Contenu,
                IsRead = false,
                ReadAt = null
            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(userId.ToString());

            return new MessageDto
            {
                Id = message.Id,
                ConversationId = message.ConversationId,
                UserId = userId,
                Contenu = message.Contenu,
                CreatedAt = message.CreatedAt,
                NomExpediteur = user?.FullName ?? "Utilisateur",
                IsRead = message.IsRead,
                ReadAt = message.ReadAt
            };
        }

    }

}
