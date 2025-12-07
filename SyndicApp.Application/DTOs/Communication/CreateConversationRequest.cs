using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Communication
{
    public class CreateConversationRequest
    {
        public string Sujet { get; set; } = string.Empty;
        public List<Guid> ParticipantsIds { get; set; } = new();
    }
}
