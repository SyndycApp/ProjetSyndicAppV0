using Microsoft.AspNetCore.Http;
using System;

namespace SyndicApp.API.Requests
{
    public class UploadAbsenceJustificatifRequest
    {
        public Guid JustificationId { get; set; }
        public IFormFile File { get; set; } = default!;
    }
}
