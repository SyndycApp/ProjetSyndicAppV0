using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Assemblees;

public class ConvocationDestinataire : BaseEntity
{
    public Guid ConvocationId { get; set; }
    public Convocation Convocation { get; set; } = null!;

    public Guid UserId { get; set; }

    public bool EstLu { get; set; }
    public DateTime? DateLecture { get; set; }

    public DateTime? LuLe { get; set; }
    public DateTime? RelanceLe { get; set; }
}
