using SyndicApp.Domain.Entities.Common;
using System;

namespace SyndicApp.Domain.Entities.Assemblees;

public class Vote : BaseEntity
{
    public Guid AssembleeGeneraleId { get; set; }
    public AssembleeGenerale AssembleeGenerale { get; set; } = null!;

    public Guid UserId { get; set; }

    public string Question { get; set; } = string.Empty;
    public string Choix { get; set; } = string.Empty; // Oui / Non / Abstention
    public DateTime DateVote { get; set; }
}
