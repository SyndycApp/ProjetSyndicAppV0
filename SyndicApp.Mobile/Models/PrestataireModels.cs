using System;

namespace SyndicApp.Mobile.Models
{
    public class PrestataireDto
    {
        public Guid Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string? TypeService { get; set; }
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? Adresse { get; set; }
        public string? Notes { get; set; }
        public int NbInterventions { get; set; }
    }

    public class PrestataireCreateRequest
    {
        public string Nom { get; set; } = string.Empty;
        public string? TypeService { get; set; }
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? Adresse { get; set; }
        public string? Notes { get; set; }
    }

    public class PrestataireUpdateRequest : PrestataireCreateRequest { }
}
