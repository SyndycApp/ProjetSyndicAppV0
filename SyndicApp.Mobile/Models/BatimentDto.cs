namespace SyndicApp.Mobile.Models;

public class BatimentDto
{
    public Guid Id { get; set; }
    public string? Nom { get; set; }
    public Guid ResidenceId { get; set; }
    public string? ResidenceNom { get; set; } // pratique pour l’affichage

    public int NbLots { get; set; }
    public int NbEtages { get; set; }
    public string ResponsableNom { get; set; }
}

public class BatimentCreateDto
{
    public string? Nom { get; set; }
    public Guid ResidenceId { get; set; }
}

public class BatimentUpdateDto : BatimentCreateDto { }
