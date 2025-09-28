using System;

namespace SyndicApp.Application.DTOs.Residences
{
    public class LocataireTemporaireDto
    {
        public Guid Id { get; set; }
        public Guid LotId { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
    }

    public class CreateLocataireTemporaireDto
    {
        public Guid LotId { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
    }

    public class UpdateLocataireTemporaireDto : CreateLocataireTemporaireDto { }
}
