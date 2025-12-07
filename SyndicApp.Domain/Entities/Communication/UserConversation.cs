using System;

namespace SyndicApp.Domain.Entities.Communication
{
    public class UserConversation
    {
        public Guid UserId { get; set; } 

        public Guid ConversationId { get; set; }
        public Conversation Conversation { get; set; } = null!;
    }
}
