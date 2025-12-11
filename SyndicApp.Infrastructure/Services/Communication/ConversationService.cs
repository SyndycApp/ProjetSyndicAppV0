using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Communication;
using SyndicApp.Application.Interfaces.Communication;
using SyndicApp.Domain.Entities.Communication;
using SyndicApp.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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
            // 1️⃣ Récupération des conversations où l'utilisateur participe
            var conversations = await _db.UserConversations
                .Where(uc => uc.UserId == userId)
                .Select(uc => uc.Conversation)
                .ToListAsync();

            var result = new List<ConversationDto>();

            foreach (var conv in conversations)
            {
                // 2️⃣ Récupérer le dernier message
                var lastMessage = await _db.Messages
                    .Where(m => m.ConversationId == conv.Id)
                    .OrderByDescending(m => m.CreatedAt)
                    .FirstOrDefaultAsync();

                // 3️⃣ Récupérer les participants
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

                // 🔥 Si une conversation n'a qu'un seul participant → PROBLÈME UI
                // On ne la supprime pas, mais on sécurise l'affichage
                if (participants.Count == 1)
                {
                    participants.Add(new ParticipantDto
                    {
                        UserId = Guid.Empty,
                        NomComplet = "Utilisateur"
                    });
                }

                // 4️⃣ Construction de la DTO
                result.Add(new ConversationDto
                {
                    Id = conv.Id,
                    Sujet = conv.Sujet,
                    DateCreation = conv.DateCreation,
                    Participants = participants,

                    DernierMessage = lastMessage == null ? null : new MessageDto
                    {
                        Id = lastMessage.Id,

                        // 🔥🔥 FIX MAJEUR : ConversationId doit être renvoyé !
                        ConversationId = lastMessage.ConversationId,

                        UserId = lastMessage.UserId,
                        Contenu = lastMessage.Contenu,
                        CreatedAt = lastMessage.CreatedAt,
                        IsRead = lastMessage.IsRead,       
                        ReadAt = lastMessage.ReadAt,
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

            if (request.ParticipantsIds.Count < 2)
                throw new Exception("Une conversation doit avoir au moins 2 participants.");

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

            var participants = await _db.Users
                .Where(u => request.ParticipantsIds.Contains(u.Id))
                .Select(u => new ParticipantDto
                {
                    UserId = u.Id,
                    NomComplet = u.FullName
                })
                .ToListAsync();

            return new ConversationDto
            {
                Id = conversation.Id,
                Sujet = conversation.Sujet,
                DateCreation = conversation.DateCreation,
                Participants = participants,
                DernierMessage = null // 🔥 IMPORTANT : nouvel objet = aucun message envoyé
            };
        }
    }
}
