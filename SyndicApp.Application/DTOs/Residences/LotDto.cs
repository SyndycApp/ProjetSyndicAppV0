using System;

namespace SyndicApp.Application.DTOs.Residences
{
    public class LotDto
    {
        public Guid Id { get; set; }
        public string NumeroLot { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public double Surface { get; set; }
        public Guid ResidenceId { get; set; }

        // Optionnels (dérivés)
        public Guid? BatimentId { get; set; } // via shadow FK si renseigné plus tard
        public string? BatimentNom { get; set; }
    }

    public class CreateLotDto
    {
        public string NumeroLot { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public double Surface { get; set; }
        public Guid ResidenceId { get; set; }
        public Guid? BatimentId { get; set; } // mappé en shadow si présent
    }

    public class UpdateLotDto : CreateLotDto { }
}
