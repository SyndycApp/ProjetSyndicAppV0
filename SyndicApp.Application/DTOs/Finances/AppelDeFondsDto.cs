// SyndicApp.Application/DTOs/Finances/AppelDeFondsDtos.cs
using System;
using System.Collections.Generic;

namespace SyndicApp.Application.DTOs.Finances
{
    public class AppelDeFondsDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal MontantTotal { get; set; }
        public DateTime DateEmission { get; set; }

        public Guid ResidenceId { get; set; }

        public string ResidenceNom { get; set; } = string.Empty;

        // Infos de synthèse utiles côté UI
        public int NbPaiements { get; set; }
        public decimal MontantPaye { get; set; }
        public decimal MontantReste => MontantTotal - MontantPaye;

        // Optionnel : aperçu des paiements liés (peut rester vide selon l’endpoint)
        public List<PaiementDto> Paiements { get; set; } = new();
    }

    public class CreateAppelDeFondsDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal MontantTotal { get; set; }
        public DateTime DateEmission { get; set; }
        public Guid ResidenceId { get; set; }
    }

    public class UpdateAppelDeFondsDto : CreateAppelDeFondsDto { }
}
