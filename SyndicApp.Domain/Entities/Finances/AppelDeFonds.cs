using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Finances;
using SyndicApp.Domain.Entities.Residences;
using System;
using System.Collections.Generic;

namespace SyndicApp.Domain.Entities.Finances;

public class AppelDeFonds : BaseEntity
{
    public string Description { get; set; } = string.Empty;
    public decimal MontantTotal { get; set; }
    public DateTime DateEmission { get; set; }

    public Guid ResidenceId { get; set; }
    public Residence Residence { get; set; } = null!;

    public ICollection<Paiement> Paiements { get; set; } = new List<Paiement>();
}
