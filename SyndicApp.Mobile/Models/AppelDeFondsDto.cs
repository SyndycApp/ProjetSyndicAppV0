namespace SyndicApp.Mobile.Models
{
    public class AppelDeFondsDto
    {
        public string Id { get; set; } = string.Empty;          // ← Guid → string
        public string? Description { get; set; }
        public decimal MontantTotal { get; set; }
        public DateTime DateEmission { get; set; }
        public string ResidenceId { get; set; } = string.Empty;  // ← Guid → string

        // synthèse
        public int NbPaiements { get; set; }
        public decimal MontantPaye { get; set; }
        public decimal MontantReste { get; set; }

        public string ResidenceNom { get; set; } = string.Empty;
        // détails
        public List<PaiementDto> Paiements { get; set; } = new();
    }
}
