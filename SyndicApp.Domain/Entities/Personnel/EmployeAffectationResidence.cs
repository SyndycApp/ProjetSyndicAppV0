using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Residences;

namespace SyndicApp.Domain.Entities.Personnel;

public class EmployeAffectationResidence : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid ResidenceId { get; set; }
    public Residence Residence { get; set; } = null!;

    public DateTime DateDebut { get; set; }
    public DateTime? DateFin { get; set; }

    public string RoleSurSite { get; set; } = string.Empty;
}
