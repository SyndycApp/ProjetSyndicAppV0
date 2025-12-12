using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Communication;
using SyndicApp.Application.Interfaces.Communication;
using SyndicApp.Domain.Entities.Communication;
using SyndicApp.Infrastructure.Identity;

namespace SyndicApp.Infrastructure.Services.Communication
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }


        // ===========================================================
        // 🔵 1) Lister TOUS les utilisateurs sauf moi (WhatsApp list)
        // ===========================================================
        public async Task<List<UserChatDto>> GetAllUsersExceptAsync(Guid userId)
        {
            return await _userManager.Users
                .Where(u => u.Id != userId)
                .Select(u => new UserChatDto
                {
                    UserId = u.Id,
                    NomComplet = u.FullName
                })
                .ToListAsync();
        }


        // ==================================================================
        // 🔵 2) Ouvrir conversation ou la créer automatiquement (like WhatsApp)
        // ==================================================================
        public async Task<Guid> OpenOrCreateConversationAsync(Guid currentUserId, Guid otherUserId)
        {
            // 1) Vérifier si conversation existe déjà entre les 2
            var existingConversationId = await _db.UserConversations
                .GroupBy(uc => uc.ConversationId)
                .Where(g => g.Any(u => u.UserId == currentUserId) &&
                            g.Any(u => u.UserId == otherUserId))
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            if (existingConversationId != Guid.Empty)
                return existingConversationId;


            // 2) Sinon → créer nouvelle conversation vide
            var conversation = new Conversation
            {
                Sujet = "Conversation privée",
                DateCreation = DateTime.UtcNow
            };

            _db.Conversations.Add(conversation);

            conversation.UserConversations.Add(new UserConversation { UserId = currentUserId });
            conversation.UserConversations.Add(new UserConversation { UserId = otherUserId });

            await _db.SaveChangesAsync();

            return conversation.Id;
        }
    }
}
