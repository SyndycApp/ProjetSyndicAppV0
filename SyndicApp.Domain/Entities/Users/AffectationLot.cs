using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Residences;
using System;

namespace SyndicApp.Domain.Entities.Users;

public class AffectationLot : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid LotId { get; set; }
    public Lot Lot { get; set; } = null!;

    public DateTime DateDebut { get; set; }
    public DateTime? DateFin { get; set; }

    public bool EstProprietaire { get; set; }
}

