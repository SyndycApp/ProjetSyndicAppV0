using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Assemblees;

public class AnnotationAssemblee : BaseEntity
{
    public Guid AssembleeGeneraleId { get; set; }
    public Guid AuteurId { get; set; } // Syndic
    public string Contenu { get; set; } = null!;
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;
    public DateTime? DateModification { get; set; }

    public AssembleeGenerale AssembleeGenerale { get; set; } = null!;
}
