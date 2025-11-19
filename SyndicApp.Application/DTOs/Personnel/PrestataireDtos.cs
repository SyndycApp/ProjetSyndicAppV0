using System;

namespace SyndicApp.Application.DTOs.Personnel
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

        // Optionnel : nombre d'interventions associées
        public int NbInterventions { get; set; }
    }

    public class PrestataireCreateDto
    {
        public string Nom { get; set; } = string.Empty;
        public string? TypeService { get; set; }
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? Adresse { get; set; }
        public string? Notes { get; set; }
    }

    public class PrestataireUpdateDto
    {
        public string Nom { get; set; } = string.Empty;
        public string? TypeService { get; set; }
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? Adresse { get; set; }
        public string? Notes { get; set; }
    }
}
