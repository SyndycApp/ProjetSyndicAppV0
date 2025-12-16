using SyndicApp.Domain.Entities.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Communication
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Guid UserId { get; set; }

        public string NomExpediteur { get; set; } = string.Empty;

       
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Contenu { get; set; }

        public string? AudioUrl { get; set; }
        public string? FileUrl { get; set; }        
        public string? FileName { get; set; }       
        public string? ContentType { get; set; }    

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public MessageType Type { get; set; }

        public MessageDto? ReplyToMessage { get; set; }

        public List<MessageReactionDto> Reactions { get; set; }
    = new();
    }

}
