using SyndicApp.Domain.Entities.Communication;
using SyndicApp.Domain.Entities.Users;
using System;

namespace SyndicApp.Domain.Entities.Users;

public class UserConversation
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid ConversationId { get; set; }
    public Conversation Conversation { get; set; } = null!;
}
