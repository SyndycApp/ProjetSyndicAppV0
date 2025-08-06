using System;

namespace SyndicApp.Application.DTOs.Residences
{
    public class CreateLotDto
    {
        public string NumeroLot { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Appartement, Garage, etc.
        public double Surface { get; set; }

        public Guid ResidenceId { get; set; }
    }
}
