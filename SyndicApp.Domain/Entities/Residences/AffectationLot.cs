using SyndicApp.Domain.Entities.Common;
using System;

namespace SyndicApp.Domain.Entities.Residences
{
    public class AffectationLot : BaseEntity
    {
        // FK vers AspNetUsers(Id)
        public Guid UserId { get; set; }

        // FK vers Lots(Id)
        public Guid LotId { get; set; }
        public Lot Lot { get; set; } = null!;

        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }

        public bool EstProprietaire { get; set; }
    }
}
