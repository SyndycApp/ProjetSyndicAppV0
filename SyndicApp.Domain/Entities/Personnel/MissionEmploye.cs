using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Personnel;

public class MissionEmploye : BaseEntity
{
    public Guid UserId { get; set; }
    public string Libelle { get; set; } = string.Empty;
}
