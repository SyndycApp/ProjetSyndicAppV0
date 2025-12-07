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
    public class ConversationService : IConversationService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConversationService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<List<ConversationDto>> GetUserConversationsAsync(Guid userId)
        {
            // 1. Récupération des conversations de l'utilisateur
            var conversations = await _db.UserConversations
                .Where(uc => uc.UserId == userId)
                .Select(uc => uc.Conversation)
                .ToListAsync();

            var result = new List<ConversationDto>();

            foreach (var conv in conversations)
            {
                // 2. Récupérer le dernier message SANS Include()
                var lastMessage = await _db.Messages
                    .Where(m => m.ConversationId == conv.Id)
                    .OrderByDescending(m => m.CreatedAt)
                    .FirstOrDefaultAsync();

                // 3. Récupérer les participants
                var participantIds = await _db.UserConversations
                    .Where(uc => uc.ConversationId == conv.Id)
                    .Select(uc => uc.UserId)
                    .ToListAsync();

                var participants = await _userManager.Users
                    .Where(u => participantIds.Contains(u.Id))
                    .Select(u => new ParticipantDto
                    {
                        UserId = u.Id,
                        NomComplet = u.FullName
                    })
                    .ToListAsync();

                // 4. Construire la DTO
                result.Add(new ConversationDto
                {
                    Id = conv.Id,
                    Sujet = conv.Sujet,
                    DateCreation = conv.DateCreation,
                    Participants = participants,
                    DernierMessage = lastMessage == null ? null : new MessageDto
                    {
                        Id = lastMessage.Id,
                        UserId = lastMessage.UserId,
                        Contenu = lastMessage.Contenu,
                        CreatedAt = lastMessage.CreatedAt,
                        NomExpediteur = participants
                            .FirstOrDefault(p => p.UserId == lastMessage.UserId)
                            ?.NomComplet ?? "Utilisateur"
                    }
                });
            }

            return result;
        }


        public async Task<ConversationDto> CreateConversationAsync(Guid creatorId, CreateConversationRequest request)
        {
            if (!request.ParticipantsIds.Contains(creatorId))
                request.ParticipantsIds.Add(creatorId);

            var conversation = new Conversation
            {
                Sujet = request.Sujet
            };

            foreach (var id in request.ParticipantsIds)
            {
                conversation.UserConversations.Add(new UserConversation
                {
                    UserId = id
                });
            }

            _db.Conversations.Add(conversation);
            await _db.SaveChangesAsync();

            return new ConversationDto
            {
                Id = conversation.Id,
                Sujet = conversation.Sujet,
                DateCreation = conversation.DateCreation,
                Participants = await _db.Users
                    .Where(u => request.ParticipantsIds.Contains(u.Id))
                    .Select(u => new ParticipantDto
                    {
                        UserId = u.Id,
                        NomComplet = u.FullName
                    }).ToListAsync()
            };
        }
    }

}
