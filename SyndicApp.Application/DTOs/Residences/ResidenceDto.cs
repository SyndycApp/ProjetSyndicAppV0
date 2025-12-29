using System;

namespace SyndicApp.Application.DTOs.Residences
{
    public class ResidenceDto
    {
        public Guid Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
        public string Ville { get; set; } = string.Empty;
        public string CodePostal { get; set; } = string.Empty;

        // Infos de synthèse
        public int NbBatiments { get; set; }
        public int NbLots { get; set; }
        public int NbIncidents { get; set; }
    }

    public class CreateResidenceDto
    {
        public string Nom { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
        public string Ville { get; set; } = string.Empty;
        public string CodePostal { get; set; } = string.Empty;

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? RayonAutoriseMetres { get; set; }
    }

    public class UpdateResidenceDto : CreateResidenceDto { }

    public class ResidenceOccupantDto
    {
        public Guid LotId { get; set; }
        public string NumeroLot { get; set; } = string.Empty;

        public Guid? UserId { get; set; }          
        public string? NomComplet { get; set; }    
        public bool? EstProprietaire { get; set; }
        public DateTime? DateDebut { get; set; }  
        public DateTime? DateFin { get; set; }    
    }

    public  class LotWithOccupantDto
    {
        public Guid LotId { get; set; }
        public string NumeroLot { get; set; } = string.Empty;
        public string? Type { get; set; }
        public double? Surface { get; set; }

        public Guid ResidenceId { get; set; }      
        public Guid? BatimentId { get; set; }

        public Guid? OccupantUserId { get; set; }
        public string? OccupantNomComplet { get; set; }
        public bool? EstProprietaire { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
    }

    public  class ResidenceDetailsDto
    {
        public Guid ResidenceId { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
        public string Ville { get; set; } = string.Empty;
        public string CodePostal { get; set; } = string.Empty;
        public int NbLots { get; set; }
        public int NbOccupantsActifs { get; set; }
        public List<LotWithOccupantDto> Lots { get; set; } = new();
    }

}
