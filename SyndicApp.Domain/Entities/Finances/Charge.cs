using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Residences;
using System;

namespace SyndicApp.Domain.Entities.Finances;

public class Charge : BaseEntity
{
    public string Nom { get; set; } = string.Empty;  // ex: Eau, Electricité, Entretien
    public decimal Montant { get; set; }
    public DateTime DateCharge { get; set; }

    public Guid ResidenceId { get; set; }
    public Residence Residence { get; set; } = null!;

    public Guid? LotId { get; set; }           // <— NEW (nullable)
    public Lot? Lot { get; set; }
}
