using System;

namespace SyndicApp.Application.DTOs.Finances
{
    public class ChargeDto
    {
        public Guid Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public decimal Montant { get; set; }
        public DateTime DateCharge { get; set; }

        public Guid ResidenceId { get; set; }
    }

    public class CreateChargeDto
    {
        public string Nom { get; set; } = string.Empty;
        public decimal Montant { get; set; }
        public DateTime DateCharge { get; set; }
        public Guid ResidenceId { get; set; }

    }

    public class UpdateChargeDto : CreateChargeDto { }
}
