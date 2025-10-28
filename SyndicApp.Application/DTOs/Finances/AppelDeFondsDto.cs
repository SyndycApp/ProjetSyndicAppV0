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

        // Infos de synth�se utiles c�t� UI
        public int NbPaiements { get; set; }
        public decimal MontantPaye { get; set; }
        public decimal MontantReste => MontantTotal - MontantPaye;

        // Optionnel : aper�u des paiements li�s (peut rester vide selon l�endpoint)
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
