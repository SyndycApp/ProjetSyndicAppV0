using System;
using System.Collections.Generic;

namespace SyndicApp.Mobile.Models
{
    public class DevisTravauxDto
    {
        public Guid Id { get; set; }
        public string? Titre { get; set; }
        public string? Description { get; set; }

        public decimal MontantHT { get; set; }
        public decimal TauxTVA { get; set; }
        public decimal MontantTTC { get; set; }

        public Guid ResidenceId { get; set; }
        public Guid? IncidentId { get; set; }

        public string Statut { get; set; } = "EnAttente";
        public DateTime DateEmission { get; set; }

        public Guid? ValideParId { get; set; }
        public DateTime? DateDecision { get; set; }
        public string? CommentaireDecision { get; set; }

        public List<Guid> InterventionIds { get; set; } = new();
    }

    public class DevisTravauxCreateRequest
    {
        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal MontantHT { get; set; }
        public decimal TauxTVA { get; set; }

        public Guid ResidenceId { get; set; }
        public Guid IncidentId { get; set; }
    }

    public class DevisTravauxDecisionRequest
    {
        public string Statut { get; set; } = "EnAttente";
        public Guid AuteurId { get; set; }
        public string Commentaire { get; set; } = string.Empty;
        public DateTime DateDecision { get; set; }
    }
}
