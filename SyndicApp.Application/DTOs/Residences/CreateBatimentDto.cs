using System;

namespace SyndicApp.Application.DTOs.Residences
{
    public class CreateBatimentDto
    {
        public string Nom { get; set; } = string.Empty;
        public Guid ResidenceId { get; set; }
    }
}
