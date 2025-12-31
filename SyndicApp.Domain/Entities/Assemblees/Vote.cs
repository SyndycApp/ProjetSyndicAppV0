using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Enums.Assemblees;

namespace SyndicApp.Domain.Entities.Assemblees;

public class Vote : BaseEntity
{
    public Guid ResolutionId { get; set; }
    public Resolution Resolution { get; set; } = null!;

    public Guid UserId { get; set; }
    public Guid LotId { get; set; }

    public ChoixVote Choix { get; set; }

    public decimal PoidsVote { get; set; } 

    public DateTime DateVote { get; set; }
    public bool EstModifie { get; set; }
}
