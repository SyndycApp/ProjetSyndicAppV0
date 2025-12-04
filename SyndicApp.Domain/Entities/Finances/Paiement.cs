using SyndicApp.Domain.Entities.Common;
using System;

namespace SyndicApp.Domain.Entities.Finances
{
    public class Paiement : BaseEntity
    {
        public decimal Montant { get; set; }
        public DateTime DatePaiement { get; set; }

        public Guid AppelDeFondsId { get; set; }
        public AppelDeFonds AppelDeFonds { get; set; }

        public Guid UserId { get; set; }
    }
}
