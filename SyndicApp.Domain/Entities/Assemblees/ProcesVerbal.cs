using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Assemblees;

public class ProcesVerbal : BaseEntity
{
    public Guid AssembleeGeneraleId { get; set; }
    public AssembleeGenerale AssembleeGenerale { get; set; } = null!;

    public DateTime DateGeneration { get; set; }
    public string Contenu { get; set; } = string.Empty; 
    public string NumeroPV { get; set; } = string.Empty;

    public Guid GenereParId { get; set; } // Syndic
    public bool EstSigne { get; set; }
    public DateTime? DateSignature { get; set; }
    public string UrlPdf { get; set; } = string.Empty;

    public bool EstVerrouille { get; set; }
    public bool EstArchive { get; set; }

    public ICollection<ProcesVerbalVersion> Versions { get; set; }
    = new List<ProcesVerbalVersion>();
}
