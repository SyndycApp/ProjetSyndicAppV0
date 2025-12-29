using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Personnel;

public class Presence : BaseEntity
{
    // ================= EXISTANT (NE PAS TOUCHER) =================
    public Guid UserId { get; set; }

    public DateTime Date { get; set; }

    public DateTime? HeureDebut { get; set; }
    public DateTime? HeureFin { get; set; }

    public string? ResidenceNom { get; set; }

    // ================= LIEN MISSION (LOT 3) =================
    public Guid? PlanningMissionId { get; set; }
    public PlanningMission? PlanningMission { get; set; }

    // ================= GEOLOCALISATION (PREMIUM) =================
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public bool IsGeoValidated { get; set; }

    // ================= AUDIT / PREUVE (AJOUT SANS RÉGRESSION) =================
    public string? Anomalie { get; set; }   // ex: "Hors zone", "Mission absente"
}
