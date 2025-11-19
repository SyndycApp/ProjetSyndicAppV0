using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Documents;
using SyndicApp.Domain.Entities.Personnel;
using SyndicApp.Domain.Entities.Residences;
using System;
using System.Collections.Generic;
using SyndicApp.Domain.Entities.Incidents.Enums;

namespace SyndicApp.Domain.Entities.Incidents
{
    public class Intervention : BaseEntity
    {
        public string Description { get; set; } = string.Empty;

        public StatutIntervention Statut { get; set; } = StatutIntervention.Planifiee;

        public DateTime? DatePrevue { get; set; }
        public DateTime? DateRealisation { get; set; }

        public decimal? CoutEstime { get; set; }
        public decimal? CoutReel { get; set; }

        public Guid ResidenceId { get; set; }
        public Residence Residence { get; set; } = null!;

        public Guid? DevisTravauxId { get; set; }
        public DevisTravaux? DevisTravaux { get; set; }

        public Guid? IncidentId { get; set; }
        public Incident? Incident { get; set; }

        // --- Employé interne ---
        public Guid? EmployeId { get; set; }
        public Employe? Employe { get; set; }

        // --- Prestataire externe (nouveau) ---
        public Guid? PrestataireId { get; set; }
        public Prestataire? Prestataire { get; set; }

        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public ICollection<InterventionHistorique> Historique { get; set; } = new List<InterventionHistorique>();
    }
}
