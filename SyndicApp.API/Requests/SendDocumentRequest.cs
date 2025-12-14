using Microsoft.AspNetCore.Http;

namespace SyndicApp.API.Requests
{
    public class SendDocumentRequest
    {
        public IFormFile Document { get; set; } = null!;
    }
}
