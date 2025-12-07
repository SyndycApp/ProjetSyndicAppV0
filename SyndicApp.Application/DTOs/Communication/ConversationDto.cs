using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Communication
{
    public class ConversationDto
    {
        public Guid Id { get; set; }
        public string Sujet { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }

        public List<ParticipantDto> Participants { get; set; } = new();
        public MessageDto? DernierMessage { get; set; }
    }

    public class ParticipantDto
    {
        public Guid UserId { get; set; }
        public string NomComplet { get; set; } = string.Empty;
    }
}
