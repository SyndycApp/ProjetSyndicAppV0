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

        public ICollection<Lot> Lots { get; set; } = new List<Lot>();
    }
}
