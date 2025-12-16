using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Communication
{
    public class MessageReactionDto
    {
        public Guid UserId { get; set; }
        public string Emoji { get; set; } = string.Empty;
    }
}
