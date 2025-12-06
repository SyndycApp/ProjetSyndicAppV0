using System;

namespace SyndicApp.Application.DTOs.Residences
{
    public class BatimentDto
    {
        public Guid Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public Guid ResidenceId { get; set; }
        public int NbLots { get; set; }
        public int NbEtages { get; set; }

        public string Bloc { get; set; } = string.Empty;
        public string ResponsableNom { get; set; } = string.Empty;
        public bool HasAscenseur { get; set; }
        public int AnneeConstruction { get; set; }
        public string CodeAcces { get; set; } = string.Empty;
    }

    public class CreateBatimentDto
    {
        public string Nom { get; set; } = string.Empty;
        public Guid ResidenceId { get; set; }
        public int NbEtages { get; set; }

        public string Bloc { get; set; } = string.Empty;
        public string ResponsableNom { get; set; } = string.Empty;
        public bool HasAscenseur { get; set; }
        public int AnneeConstruction { get; set; }
        public string CodeAcces { get; set; } = string.Empty;
    }

    public class UpdateBatimentDto : CreateBatimentDto { }
}
