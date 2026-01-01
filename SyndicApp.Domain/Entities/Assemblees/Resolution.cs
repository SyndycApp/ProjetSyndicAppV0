using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Enums.Assemblees;

namespace SyndicApp.Domain.Entities.Assemblees;

public class Resolution : BaseEntity
{
    public Guid AssembleeGeneraleId { get; set; }
    public AssembleeGenerale AssembleeGenerale { get; set; } = null!;

    public int Numero { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public StatutResolution Statut { get; set; } = StatutResolution.EnAttente;

    public ICollection<Vote> Votes { get; set; } = new List<Vote>();

    // nouveaux 
    public TypeMajorite TypeMajorite { get; set; } = TypeMajorite.Simple;
    public decimal? SeuilMajorite { get; set; }
    public Decision? Decision { get; set; }
}
