namespace SyndicApp.Mobile.Models
{
    public class AppelDeFondsDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("montantTotal")]
        public decimal MontantTotal { get; set; }

        [JsonPropertyName("dateEmission")]
        public DateTime DateEmission { get; set; }

        [JsonPropertyName("residenceId")]
        public Guid ResidenceId { get; set; }

        [JsonPropertyName("residenceNom")]
        public string ResidenceNom { get; set; } = string.Empty;

        [JsonPropertyName("nbPaiements")]
        public int NbPaiements { get; set; }

        [JsonPropertyName("montantPaye")]
        public decimal MontantPaye { get; set; }

        [JsonPropertyName("montantReste")]
        public decimal MontantReste { get; set; }

        [JsonPropertyName("paiements")]
        public List<PaiementDto> Paiements { get; set; } = new();
    }
}
