// SyndicApp.Application/DTOs/Finances/PaiementDtos.cs
using System;

namespace SyndicApp.Application.DTOs.Finances
{
    public class PaiementDto
    {
        public Guid Id { get; set; }
        public decimal Montant { get; set; }
        public DateTime DatePaiement { get; set; }

        public Guid AppelDeFondsId { get; set; }
        public Guid UserId { get; set; }

        // Optionnel : infos d’affichage
        public string? NomCompletUser { get; set; }
    }

    public class CreatePaiementDto
    {
        public Guid AppelDeFondsId { get; set; }
        public Guid UserId { get; set; }
        public decimal Montant { get; set; }
        public DateTime DatePaiement { get; set; }
    }
}
