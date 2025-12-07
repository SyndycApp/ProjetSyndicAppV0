using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Communication
{
    public class SendMessageRequest
    {
        public Guid ConversationId { get; set; }
        public string Contenu { get; set; } = string.Empty;
    }
}
