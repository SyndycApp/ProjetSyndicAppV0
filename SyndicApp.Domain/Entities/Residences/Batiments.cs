using System;
using System.Collections.Generic;
using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Residences
{
    public class Batiment : BaseEntity
    {
        public string Nom { get; set; } = string.Empty;

        public Guid ResidenceId { get; set; }
        public Residence Residence { get; set; } = null!;

        public int NbEtages { get; set; }

        public string Bloc { get; set; } = string.Empty;
        public string ResponsableNom { get; set; } = string.Empty;
        public bool HasAscenseur { get; set; }
        public int AnneeConstruction { get; set; }
        public string CodeAcces { get; set; } = string.Empty;
        public ICollection<Lot> Lots { get; set; } = new List<Lot>();
    }
}
