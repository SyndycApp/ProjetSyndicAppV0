using Microsoft.AspNetCore.Http;
using SyndicApp.Domain.Enums;

namespace SyndicApp.API.Requests
{
    public class UploadEmployeDocumentRequest
    {
        public Guid EmployeId { get; set; }
        public DocumentRHType Type { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}