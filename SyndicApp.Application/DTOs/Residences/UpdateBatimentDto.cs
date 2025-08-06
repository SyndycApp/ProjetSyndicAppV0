using System;

namespace SyndicApp.Application.DTOs.Residences
{
    public class UpdateBatimentDto
    {
        public Guid Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public Guid ResidenceId { get; set; }
    }
}
