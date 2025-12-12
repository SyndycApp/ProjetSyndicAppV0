using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.Models
{
    public class OpenConversationRequest
    {
        public Guid OtherUserId { get; set; }
    }

    public class OpenConversationResponse
    {
        public Guid ConversationId { get; set; }
    }

}
