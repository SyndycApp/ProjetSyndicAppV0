using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Documents;
using SyndicApp.Domain.Entities.Residences;
using System;
using System.Collections.Generic;
using SyndicApp.Domain.Entities.Incidents.Enums;

namespace SyndicApp.Domain.Entities.Incidents
{
    public class Incident : BaseEntity
    {
        // Infos principales
        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? TypeIncident { get; set; } // ex: Ascenseur, Plomberie...
        public DateTime DateDeclaration { get; set; } = DateTime.UtcNow;

        // Cycle de vie & criticité
        public StatutIncident Statut { get; set; } = StatutIncident.Ouvert;
        public UrgenceIncident Urgence { get; set; } = UrgenceIncident.Normale;

        // Auteur (pas de navigation vers User par contrainte projet)
        public Guid DeclareParId { get; set; }

        // Ciblage (résidence obligatoire, lot optionnel)
        public Guid ResidenceId { get; set; }
        public Residence Residence { get; set; } = null!;

        public Guid? LotId { get; set; }
        public Lot? Lot { get; set; }

        // Relations fonctionnelles
        public ICollection<DevisTravaux> Devis { get; set; } = new List<DevisTravaux>();
        public ICollection<Intervention> Interventions { get; set; } = new List<Intervention>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();

        // Historique dédié
        public ICollection<IncidentHistorique> Historique { get; set; } = new List<IncidentHistorique>();
    }
}
