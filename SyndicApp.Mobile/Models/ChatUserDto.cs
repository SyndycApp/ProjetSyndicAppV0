using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.Models
{
    public class ChatUserDto
    {
        public Guid UserId { get; set; }
        public string NomComplet { get; set; } = "";
    }
}
