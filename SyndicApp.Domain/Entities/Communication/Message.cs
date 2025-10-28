using SyndicApp.Domain.Entities.Common;
using System;

namespace SyndicApp.Domain.Entities.Communication;

public class Message
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public Conversation Conversation { get; set; } = null!;

    public Guid UserId { get; set; }            
    public string Contenu { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

