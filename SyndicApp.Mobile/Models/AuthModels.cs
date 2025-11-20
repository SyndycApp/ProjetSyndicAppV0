using System;
using System.Collections.Generic;

namespace SyndicApp.Mobile.Models
{
    public class CreatePrestataireAccountRequest
    {
        public Guid PrestataireId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

}
