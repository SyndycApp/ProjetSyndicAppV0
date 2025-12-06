namespace SyndicApp.Mobile.Models;

public class BatimentDto
{
    public Guid Id { get; set; }
    public string? Nom { get; set; }
    public Guid ResidenceId { get; set; }
    public string? ResidenceNom { get; set; } // pratique pour l’affichage

    public string Bloc { get; set; } = string.Empty;
    public string ResponsableNom { get; set; } = string.Empty;
    public bool HasAscenseur { get; set; }
    public int AnneeConstruction { get; set; }
    public string CodeAcces { get; set; } = string.Empty;
    public int NbLots { get; set; }
    public int NbEtages { get; set; }
}

public class BatimentCreateDto
{
    public string? Nom { get; set; }
    public Guid ResidenceId { get; set; }
    public int NbEtages { get; set; }
    public string Bloc { get; set; } = string.Empty;
    public string ResponsableNom { get; set; } = string.Empty;
    public bool HasAscenseur { get; set; }
    public int AnneeConstruction { get; set; }
    public string CodeAcces { get; set; } = string.Empty;
}

public class BatimentUpdateDto : BatimentCreateDto { }
