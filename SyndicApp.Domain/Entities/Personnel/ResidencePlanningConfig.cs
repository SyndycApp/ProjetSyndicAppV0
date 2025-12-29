using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Residences;

public class ResidencePlanningConfig : BaseEntity
{
    public Guid ResidenceId { get; set; }
    public Residence Residence { get; set; } = null!;

    public double MaxHeuresParJour { get; set; } = 8;
}
