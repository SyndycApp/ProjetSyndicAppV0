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

        // Optionnels (d�riv�s)
        public Guid? BatimentId { get; set; } // via shadow FK si renseign� plus tard
        public string? BatimentNom { get; set; }
    }

    public class CreateLotDto
    {
        public string NumeroLot { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public double Surface { get; set; }
        public Guid ResidenceId { get; set; }
        public Guid? BatimentId { get; set; } // mapp� en shadow si pr�sent
    }

    public class UpdateLotDto : CreateLotDto { }
}
