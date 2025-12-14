using Microsoft.AspNetCore.Http;

namespace SyndicApp.API.Requests
{
    public class SendImageRequest
    {
        public IFormFile Image { get; set; } = null!;
    }
}
