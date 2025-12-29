using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Personnel;

public class EmployeProfil : BaseEntity
{
    public Guid UserId { get; set; }

    public string TypeContrat { get; set; } = string.Empty; 
    public DateTime DateDebut { get; set; }
    public DateTime? DateFin { get; set; }

    public string? Commentaire { get; set; }

    public ICollection<HoraireTravail> Horaires { get; set; } = new List<HoraireTravail>();
    public ICollection<MissionEmploye> Missions { get; set; } = new List<MissionEmploye>();
}
