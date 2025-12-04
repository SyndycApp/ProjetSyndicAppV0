namespace SyndicApp.Mobile.Models
{
    public class PaiementDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("montant")]
        public decimal Montant { get; set; }

        [JsonPropertyName("datePaiement")]
        public DateTime DatePaiement { get; set; }

        [JsonPropertyName("appelDeFondsId")]
        public Guid AppelDeFondsId { get; set; }

        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        [JsonPropertyName("nomCompletUser")]
        public string? NomCompletUser { get; set; }
    }

    public class PaiementCreateRequest
    {
        public Guid AppelDeFondsId { get; set; }
        public Guid UserId { get; set; }
        public decimal Montant { get; set; }
        public DateTime DatePaiement { get; set; }
    }
}
