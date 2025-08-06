using System;

namespace SyndicApp.Application.DTOs.Residences
{
    public class ResidenceDto
    {
        public Guid Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
        public string Ville { get; set; } = string.Empty;
        public string CodePostal { get; set; } = string.Empty;
    }
}
