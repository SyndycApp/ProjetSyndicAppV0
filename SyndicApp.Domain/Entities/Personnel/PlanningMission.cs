using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Residences;

namespace SyndicApp.Domain.Entities.Personnel;

public class PlanningMission : BaseEntity
{
    public Guid EmployeId { get; set; }
    public Employe Employe { get; set; } = null!;

    public Guid ResidenceId { get; set; }
    public Residence Residence { get; set; } = null!;

    public string Mission { get; set; } = string.Empty;

    public DateOnly Date { get; set; }
    public TimeSpan HeureDebut { get; set; }
    public TimeSpan HeureFin { get; set; }

    public string Statut { get; set; } = "Planifiee";

    public MissionValidation? Validation { get; set; }
}
