using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Assemblees;

public class Convocation : BaseEntity
{
    public Guid AssembleeGeneraleId { get; set; }
    public AssembleeGenerale AssembleeGenerale { get; set; } = null!;

    public DateTime DateEnvoi { get; set; }
    public string Contenu { get; set; } = string.Empty;

    public ICollection<ConvocationDestinataire> Destinataires { get; set; }
        = new List<ConvocationDestinataire>();
}
