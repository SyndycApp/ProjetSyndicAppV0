using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Finances;
using SyndicApp.Domain.Entities.Incidents;
using SyndicApp.Domain.Entities.LocauxCommerciaux;
using System;
using System.Collections.Generic;

namespace SyndicApp.Domain.Entities.Residences
{
    public class Lot : BaseEntity
    {
        // --- Informations principales ---
        public string NumeroLot { get; set; } = string.Empty;   // ex: A12, B03, etc.
        public string Type { get; set; } = string.Empty;        // Appartement, Garage, Local commercial...
        public double Surface { get; set; }

        // --- Rattachement Résidence ---
        public Guid ResidenceId { get; set; }
        public Residence Residence { get; set; } = null!;

        // --- Relations fonctionnelles ---
        public ICollection<AffectationLot> Affectations { get; set; } = new List<AffectationLot>();
        public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
        public ICollection<LocataireTemporaire> LocationsTemporaires { get; set; } = new List<LocataireTemporaire>();

        // --- Relations complémentaires (optionnelles mais recommandées) ---
        public ICollection<Charge> Charges { get; set; } = new List<Charge>();
        public ICollection<LocalCommercial> LocauxCommerciaux { get; set; } = new List<LocalCommercial>();
    }
}
