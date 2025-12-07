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

        public string Contenu { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
