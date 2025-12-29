using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Personnel;

public class HoraireTravail : BaseEntity
{
    public Guid EmployeProfilId { get; set; }

    public DayOfWeek Jour { get; set; }
    public TimeSpan HeureDebut { get; set; }
    public TimeSpan HeureFin { get; set; }
}
