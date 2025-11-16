namespace SyndicApp.Mobile.Models
{
    public class PaiementDto
    {
        public Guid Id { get; set; }
        public decimal Montant { get; set; }
        public DateTime DatePaiement { get; set; }
        public Guid AppelDeFondsId { get; set; }
        public Guid UserId { get; set; }

        public string? NomCompletUser { get; set; }

        public string? AppelDescription { get; set; }
    }

    public class PaiementCreateRequest
    {
        public Guid AppelDeFondsId { get; set; }
        public Guid UserId { get; set; }
        public decimal Montant { get; set; }
        public DateTime DatePaiement { get; set; }
    }
}
