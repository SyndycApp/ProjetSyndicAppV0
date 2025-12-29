using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Personnel;

public class Presence : BaseEntity
{
    public Guid UserId { get; set; }

    public DateTime Date { get; set; }

    public DateTime? HeureDebut { get; set; }
    public DateTime? HeureFin { get; set; }
    public string? ResidenceNom { get; set; }
}
