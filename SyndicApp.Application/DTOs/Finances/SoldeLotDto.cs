// SyndicApp.Application/DTOs/Finances/SoldesDtos.cs
using System;
using System.Collections.Generic;

namespace SyndicApp.Application.DTOs.Finances
{
    // Solde d’un lot (toutes créances ouvertes – paiements)
    public class SoldeLotDto
    {
        public Guid LotId { get; set; }
        public string NumeroLot { get; set; } = string.Empty;

        public decimal Du { get; set; }
        public decimal Paye { get; set; }
        public decimal Reste => Du - Paye;

        // Optionnel : drapeau pratique côté UI
        public bool EstSolde => Reste <= 0m;
    }

    // Synthèse des soldes d’une résidence (agrégation des lots)
    public class SoldeResidenceDto
    {
        public Guid ResidenceId { get; set; }
        public int NbLots { get; set; }

        public decimal DuTotal { get; set; }
        public decimal PayeTotal { get; set; }
        public decimal ResteTotal => DuTotal - PayeTotal;

        public List<SoldeLotDto> Details { get; set; } = new();
    }
}
