namespace SyndicApp.Application.DTOs.Personnel;

public class HoraireDto
{
    public string Jour { get; set; } = string.Empty;
    public TimeSpan HeureDebut { get; set; }
    public TimeSpan HeureFin { get; set; } 
}
