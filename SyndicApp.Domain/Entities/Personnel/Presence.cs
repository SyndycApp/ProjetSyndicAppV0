using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Personnel;

public class Presence : BaseEntity
{
    public Guid UserId { get; set; }

    public DateTime Date { get; set; }

    public DateTime? HeureDebut { get; set; }
    public DateTime? HeureFin { get; set; }
    public string? ResidenceNom { get; set; }

    public Guid? PlanningMissionId { get; set; }
    public PlanningMission? PlanningMission { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool IsGeoValidated { get; set; }
}
