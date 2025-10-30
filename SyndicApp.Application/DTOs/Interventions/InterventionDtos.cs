using System;
using SyndicApp.Domain.Entities.Incidents;
using SyndicApp.Domain.Entities.Incidents.Enums;

namespace SyndicApp.Application.DTOs.Interventions
{
    public class InterventionCreateDto
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

    public class InterventionChangeStatusDto
    {
        public StatutIntervention Statut { get; set; }
        public DateTime? DateRealisation { get; set; } // si Terminee
        public decimal? CoutReel { get; set; }         // si Terminee
        public Guid AuteurId { get; set; }
        public string? Commentaire { get; set; }
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
    }
}
