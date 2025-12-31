using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Assemblees;

public class ProcesVerbal : BaseEntity
{
    public Guid AssembleeGeneraleId { get; set; }
    public AssembleeGenerale AssembleeGenerale { get; set; } = null!;

    public string UrlPdf { get; set; } = string.Empty;
    public DateTime DateGeneration { get; set; }

    public bool EstArchive { get; set; }
}
