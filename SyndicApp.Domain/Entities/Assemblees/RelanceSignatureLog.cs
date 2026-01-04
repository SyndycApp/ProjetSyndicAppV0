using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Assemblees;

public class RelanceSignatureLog : BaseEntity
{
    public Guid ProcesVerbalId { get; set; }
    public Guid UserId { get; set; }
    public DateTime DateRelance { get; set; } = DateTime.UtcNow;
}
