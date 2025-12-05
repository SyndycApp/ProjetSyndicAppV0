using System;
using System.Collections.Generic;

namespace SyndicApp.Mobile.Models
{
    public class IncidentDto
    {
        public Guid Id { get; set; }
        public string? Titre { get; set; }
        public string? Description { get; set; }
        public string? TypeIncident { get; set; }
        public string Urgence { get; set; } = "Basse";
        public string Statut { get; set; } = "Ouvert";
        public DateTime DateDeclaration { get; set; }

        public Guid? ResidenceId { get; set; }
        public Guid? LotId { get; set; }
        public Guid? DeclareParId { get; set; }

        // Champs calculés pour l’affichage
        public string? ResidenceNom { get; set; }
        public string? LotNumero { get; set; }
        public string? DeclarantNomComplet { get; set; }

        // 🔎 Historique remonté par l’API
        public List<IncidentHistoriqueItem> Historique { get; set; } = new();
    }

    public class IncidentHistoriqueItem
    {
        public Guid Id { get; set; }
        public Guid IncidentId { get; set; }
        public DateTime DateAction { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? Commentaire { get; set; }
        public Guid? AuteurId { get; set; }
        public string? AuteurNom { get; set; }
    }

    public class IncidentCreateRequest
    {
        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TypeIncident { get; set; } = string.Empty;
        public string Urgence { get; set; } = "Basse";
        public Guid ResidenceId { get; set; }
        public Guid LotId { get; set; }
        public Guid DeclareParId { get; set; }
    }

    public class IncidentUpdateRequest
    {
        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TypeIncident { get; set; } = string.Empty;
        public string Urgence { get; set; } = "Basse";
        public Guid LotId { get; set; }

        public Guid? AuteurId { get; set; }
        public string Commentaire { get; set; } = string.Empty;
    }

    public class IncidentStatusUpdateRequest
    {
        public string Statut { get; set; } = "Ouvert";
        public Guid AuteurId { get; set; }
        public string Commentaire { get; set; } = string.Empty;
    }
}
