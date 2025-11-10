using System;

namespace SyndicApp.Mobile.Models
{
    public class LotDto
    {
        public Guid Id { get; set; }
        public string? NumeroLot { get; set; }
        public string? Type { get; set; }
        public double Surface { get; set; }
        public Guid ResidenceId { get; set; }
        public Guid? BatimentId { get; set; }
        public string? BatimentNom { get; set; }
    }

    public class CreateLotDto
    {
        public string? NumeroLot { get; set; }
        public string? Type { get; set; }
        public double Surface { get; set; }
        public Guid ResidenceId { get; set; }
        public Guid? BatimentId { get; set; }
    }

    public class UpdateLotDto : CreateLotDto { }

 
}
