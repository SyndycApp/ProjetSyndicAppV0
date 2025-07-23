using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Users;
using System;
using System.Collections.Generic;

namespace SyndicApp.Domain.Entities.Communication;

public class Conversation : BaseEntity
{
    public string Sujet { get; set; } = string.Empty;
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    public ICollection<UserConversation> Participants { get; set; } = new List<UserConversation>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
