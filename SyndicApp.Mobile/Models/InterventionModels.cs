// SyndicApp.Mobile/Models/InterventionModels.cs
using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace SyndicApp.Mobile.Models
{
    // ⚠️ Je suppose le même ordre d'énum que côté API
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatutIntervention
    {
        Planifiee = 0,
        EnCours = 1,
        Terminee = 2,
        Annulee = 3
    }

    public class InterventionDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;

        public Guid ResidenceId { get; set; }
        public Guid? DevisTravauxId { get; set; }
        public Guid? IncidentId { get; set; }
        public Guid? EmployeId { get; set; }
        public string? PrestataireExterne { get; set; }

        public DateTime? DatePrevue { get; set; }
        public DateTime? DateRealisation { get; set; }

        public decimal? CoutEstime { get; set; }
        public decimal? CoutReel { get; set; }

        public StatutIntervention Statut { get; set; }

        [JsonIgnore]
        public string DescriptionSansGuid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Description))
                    return string.Empty;

                // Enlève le #GUID au milieu de la phrase
                var cleaned = Regex.Replace(Description, "#[0-9a-fA-F-]+", "").Trim();

                // Nettoyage des espaces doubles / "  -"
                cleaned = cleaned.Replace("  ", " ").Replace(" -", " -").Trim();

                return cleaned;
            }
        }
    }

    // Si un jour tu veux créer une intervention depuis le mobile
    public class InterventionCreateRequest
    {
        public string Description { get; set; } = string.Empty;
        public Guid ResidenceId { get; set; }
        public Guid? DevisTravauxId { get; set; }
        public Guid? IncidentId { get; set; }
        public Guid? EmployeId { get; set; }
        public string? PrestataireExterne { get; set; }
        public DateTime? DatePrevue { get; set; }
        public decimal? CoutEstime { get; set; }
    }

    public class InterventionChangeStatusRequest
    {
        public StatutIntervention Statut { get; set; }
        public DateTime? DateRealisation { get; set; }
        public decimal? CoutReel { get; set; }
        public Guid AuteurId { get; set; }
        public string? Commentaire { get; set; }
    }
}
