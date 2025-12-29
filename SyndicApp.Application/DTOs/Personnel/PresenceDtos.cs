namespace SyndicApp.Application.DTOs.Personnel;

public class PresenceDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime? HeureDebut { get; set; }
    public DateTime? HeureFin { get; set; }
    public string? ResidenceNom { get; set; }
}

public class StartPresenceDto
{
    public string? ResidenceNom { get; set; }
}
