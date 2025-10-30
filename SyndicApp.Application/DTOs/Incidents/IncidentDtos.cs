using System;
using System.Collections.Generic;
using SyndicApp.Domain.Entities.Incidents;
using SyndicApp.Domain.Entities.Incidents.Enums;

namespace SyndicApp.Application.DTOs.Incidents
{
    public class IncidentCreateDto
    {
        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? TypeIncident { get; set; }
        public UrgenceIncident Urgence { get; set; } = UrgenceIncident.Normale;

        public Guid ResidenceId { get; set; }
        public Guid? LotId { get; set; }

        // pas de nav User : juste l�ID du d�clarant
        public Guid DeclareParId { get; set; }
    }

    public class IncidentUpdateDto
    {
        public string? Titre { get; set; }
        public string? Description { get; set; }
        public string? TypeIncident { get; set; }
        public UrgenceIncident? Urgence { get; set; }
        public Guid? LotId { get; set; }
    }

    public class IncidentChangeStatusDto
    {
        public StatutIncident Statut { get; set; }
        public Guid AuteurId { get; set; } // pour historique
        public string? Commentaire { get; set; }
    }

    public class IncidentDto
    {
        public Guid Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? TypeIncident { get; set; }
        public UrgenceIncident Urgence { get; set; }
        public StatutIncident Statut { get; set; }
        public DateTime DateDeclaration { get; set; }

        public Guid ResidenceId { get; set; }
        public Guid? LotId { get; set; }
        public Guid DeclareParId { get; set; }

        public List<Guid> DevisIds { get; set; } = new();
        public List<Guid> InterventionIds { get; set; } = new();
    }
}
