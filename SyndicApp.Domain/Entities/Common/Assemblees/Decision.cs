using SyndicApp.Domain.Entities.Common;
using System;

namespace SyndicApp.Domain.Entities.Assemblees;

public class Decision : BaseEntity
{
    public Guid AssembleeGeneraleId { get; set; }
    public AssembleeGenerale AssembleeGenerale { get; set; } = null!;

    public string Titre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateAdoption { get; set; }
}
