using SyndicApp.Domain.Entities.Common;
using System;

namespace SyndicApp.Domain.Entities.Communication
{
    public class Message : BaseEntity
    {
        public Guid ConversationId { get; set; }
        public Conversation Conversation { get; set; } = null!;

        public Guid UserId { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime? ReadAt { get; set; }
        public string? Contenu { get; set; } = string.Empty;

        public string? AudioPath { get; set; }   // ex: /uploads/audio/xxx.m4a

        public MessageType Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
