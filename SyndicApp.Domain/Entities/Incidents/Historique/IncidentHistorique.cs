using SyndicApp.Domain.Entities.Common;
using System;

namespace SyndicApp.Domain.Entities.Incidents
{
    public class IncidentHistorique : BaseEntity
    {
        public Guid IncidentId { get; set; }
        public Incident Incident { get; set; } = null!;

        public DateTime DateAction { get; set; } = DateTime.UtcNow;
        public string Action { get; set; } = string.Empty;      // ex: "Statut: Ouvert ? EnCours"
        public string? Commentaire { get; set; }

        // Traçabilité auteur (pas de nav vers User)
        public Guid AuteurId { get; set; }
    }
}
