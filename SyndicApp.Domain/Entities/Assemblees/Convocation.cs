using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Assemblees;

public class Convocation : BaseEntity
{
    public Guid AssembleeGeneraleId { get; set; }
    public AssembleeGenerale AssembleeGenerale { get; set; } = null!;

    public DateTime DateEnvoi { get; set; }
    public string Contenu { get; set; } = string.Empty;

    public ICollection<ConvocationEnvoiLog> Envois { get; set; } = new List<ConvocationEnvoiLog>();
    public ICollection<ConvocationPieceJointe> PiecesJointes { get; set; } = new List<ConvocationPieceJointe>();
    public ICollection<ConvocationDestinataire> Destinataires { get; set; } = new List<ConvocationDestinataire>();
}
