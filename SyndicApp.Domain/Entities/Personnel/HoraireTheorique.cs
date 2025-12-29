using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Personnel;

public class HoraireTheorique : BaseEntity
{
    public Guid UserId { get; set; }

    public DayOfWeek Jour { get; set; }
    public TimeSpan HeureDebut { get; set; }
    public TimeSpan HeureFin { get; set; }
}
