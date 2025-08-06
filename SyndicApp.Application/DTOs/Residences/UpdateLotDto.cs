using System;

namespace SyndicApp.Application.DTOs.Residences
{
    public class UpdateLotDto
    {
        public Guid Id { get; set; }

        public string NumeroLot { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public double Surface { get; set; }

        public Guid ResidenceId { get; set; }
    }
}
