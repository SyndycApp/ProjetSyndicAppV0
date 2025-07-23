using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Users;
using System;

namespace SyndicApp.Domain.Entities.Communication;

public class Message : BaseEntity
{
    public string Contenu { get; set; } = string.Empty;
    public DateTime DateEnvoi { get; set; } = DateTime.UtcNow;

    public Guid ConversationId { get; set; }
    public Conversation Conversation { get; set; } = null!;

    public Guid ExpediteurId { get; set; }
    public User Expediteur { get; set; } = null!;
}
