using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Finances;
using SyndicApp.Domain.Entities.Users;
using System;

namespace SyndicApp.Domain.Entities.Finances;

public class Paiement : BaseEntity
{
    public decimal Montant { get; set; }
    public DateTime DatePaiement { get; set; }

    public Guid AppelDeFondsId { get; set; }
    public AppelDeFonds AppelDeFonds { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
