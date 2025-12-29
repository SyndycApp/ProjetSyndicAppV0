namespace SyndicApp.Mobile.Models;

public class EmployeDetailsDto
{
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";

    public string TypeContrat { get; set; } = "";
    public DateTime DateDebutContrat { get; set; }
    public DateTime? DateFinContrat { get; set; }

    public List<HoraireDto> Horaires { get; set; } = [];
    public List<string> Missions { get; set; } = [];
    public List<string> Residences { get; set; } = [];
}
