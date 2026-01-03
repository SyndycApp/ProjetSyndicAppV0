using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Assemblees;

public class OrdreDuJourItem : BaseEntity
{
    public Guid AssembleeGeneraleId { get; set; }
    public AssembleeGenerale AssembleeGenerale { get; set; } = null!;

    public int Ordre { get; set; }

    public string Titre { get; set; } = null!;
    public string? Description { get; set; }
}
