namespace SyndicApp.Mobile.Models
{
    public class UpdateAppelDeFondsRequest
    {
        public string Id { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal MontantTotal { get; set; }
        public DateTime DateEmission { get; set; }
        public Guid ResidenceId { get; set; }

        public int NbPaiements { get; set; }
        public decimal MontantPaye { get; set; }
        public decimal MontantReste { get; set; }
    }

    public class CreateAppelDeFondsRequest
    {
        public string? Description { get; set; }
        public decimal MontantTotal { get; set; }
        public DateTime DateEmission { get; set; }
        public Guid ResidenceId { get; set; }
    }
}
