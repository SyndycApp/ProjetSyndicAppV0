using System;

namespace SyndicApp.Mobile.Models
{
    public class ChargeDto
    {
        public Guid Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public decimal Montant { get; set; }
        public DateTime DateCharge { get; set; }
        public Guid ResidenceId { get; set; }
        public string ResidenceNom { get; set; } = string.Empty; 
    }

    public class ChargeCreateRequest
    {
        public string Nom { get; set; } = string.Empty;
        public decimal Montant { get; set; }
        public DateTime DateCharge { get; set; }
        public Guid ResidenceId { get; set; }
    }

    public class ChargeUpdateRequest
    {
        public string Nom { get; set; } = string.Empty;
        public decimal Montant { get; set; }
        public DateTime DateCharge { get; set; }
        public Guid ResidenceId { get; set; }
    }

}
