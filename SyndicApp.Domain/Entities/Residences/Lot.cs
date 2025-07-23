using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Users;
using System;
using System.Collections.Generic;

namespace SyndicApp.Domain.Entities.Residences;

public class Lot : BaseEntity
{
    public string NumeroLot { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Ex : Appartement, Garage, Local Commercial
    public double Surface { get; set; }

    public Guid ResidenceId { get; set; }
    public Residence Residence { get; set; } = null!;

    public ICollection<AffectationLot> Affectations { get; set; } = new List<AffectationLot>();
    public ICollection<LocataireTemporaire> LocationsTemp { get; set; } = new List<LocataireTemporaire>();
}
