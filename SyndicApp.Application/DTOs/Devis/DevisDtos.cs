using System;
using System.Collections.Generic;
using SyndicApp.Domain.Entities.Incidents;
using SyndicApp.Domain.Entities.Incidents.Enums;

namespace SyndicApp.Application.DTOs.Devis
{
    public class DevisCreateDto
    {
        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal MontantHT { get; set; }
        public decimal TauxTVA { get; set; } = 0.20m;

        public Guid ResidenceId { get; set; }
        public Guid? IncidentId { get; set; }
    }

    public class DevisDecisionDto
    {
        public StatutDevis Statut { get; set; } // Accepte / Refuse / TravauxEnCours / Termine
        public Guid AuteurId { get; set; }      // Syndic qui décide
        public string? Commentaire { get; set; }
        public DateTime? DateDecision { get; set; }
    }

    public class DevisDto
    {
        public Guid Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal MontantHT { get; set; }
        public decimal TauxTVA { get; set; }
        public decimal MontantTTC { get; set; }

        public Guid ResidenceId { get; set; }
        public Guid? IncidentId { get; set; }
        public StatutDevis Statut { get; set; }
        public DateTime DateEmission { get; set; }

        public Guid? ValideParId { get; set; }
        public DateTime? DateDecision { get; set; }
        public string? CommentaireDecision { get; set; }

        public List<Guid> InterventionIds { get; set; } = new();
    }
}
