using Microsoft.AspNetCore.Http;

namespace SyndicApp.API.Requests
{
    public class SendAudioFormRequest
    {
        public Guid ConversationId { get; set; }
        public IFormFile AudioFile { get; set; } = default!;
    }
}
