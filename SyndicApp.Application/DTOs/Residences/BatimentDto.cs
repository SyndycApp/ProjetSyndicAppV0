using System;

namespace SyndicApp.Application.DTOs.Residences
{
    public class BatimentDto
    {
        public Guid Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public Guid ResidenceId { get; set; }
        public int NbLots { get; set; }
    }

    public class CreateBatimentDto
    {
        public string Nom { get; set; } = string.Empty;
        public Guid ResidenceId { get; set; }
    }

    public class UpdateBatimentDto : CreateBatimentDto { }
}
