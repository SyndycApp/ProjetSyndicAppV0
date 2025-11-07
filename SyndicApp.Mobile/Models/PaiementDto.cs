namespace SyndicApp.Mobile.Models
{
    public class PaiementDto
    {
        public string Id { get; set; } = string.Empty;  // ← Guid → string
        public DateTime DatePaiement { get; set; }
        public decimal Montant { get; set; }
        public string? Statut { get; set; }             // "Reçu" | "En retard" | ...
        public string? UserId { get; set; }             // ← Guid? → string?
        public string? LotId { get; set; }             // ← Guid? → string?
    }
}
