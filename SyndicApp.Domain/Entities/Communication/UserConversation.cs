using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Users;
using System;

namespace SyndicApp.Domain.Entities.Communication
{
    public class UserConversation
    {
        public Guid UserId { get; set; }
        public Guid ConversationId { get; set; }

        // Relations
        public User User { get; set; } = null!;
        public Conversation Conversation { get; set; } = null!;
    }
}
