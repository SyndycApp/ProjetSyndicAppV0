using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Users;
using System;

namespace SyndicApp.Domain.Entities.Assemblees;

public class Convocation : BaseEntity
{
    public Guid AssembleeGeneraleId { get; set; }
    public AssembleeGenerale AssembleeGenerale { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public DateTime DateEnvoi { get; set; }
    public bool EstLu { get; set; }
}
