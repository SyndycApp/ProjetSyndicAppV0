namespace SyndicApp.Mobile.Models;

public class ResidenceDto
{
    public Guid Id { get; set; }
    public string? Nom { get; set; }
    public string? Adresse { get; set; }
    public string? Ville { get; set; }
    public string? CodePostal { get; set; }
}

public  class ResidenceCreateDto
{
    public string? Nom { get; set; }
    public string? Adresse { get; set; }
    public string? Ville { get; set; }
    public string? CodePostal { get; set; }
}

public class ResidenceUpdateDto : ResidenceCreateDto { }

public class ResidenceDetailsDto : ResidenceDto
{
    // ajoute si besoin: liste de lots, occupants, etc.
}

 public class ResidenceLookupDto
 {
        // adapte selon ton API, mais ça compiles déjà comme ça
        public string Id { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
 }

