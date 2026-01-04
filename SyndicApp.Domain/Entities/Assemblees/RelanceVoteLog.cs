using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Assemblees;

public class RelanceVoteLog : BaseEntity
{
    public Guid AssembleeGeneraleId { get; set; }
    public Guid UserId { get; set; }
    public DateTime DateRelance { get; set; } = DateTime.UtcNow;
    public string Type { get; set; } = "NON_VOTANT";
}
