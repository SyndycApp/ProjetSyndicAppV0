using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Assemblees;

public class ProcesVerbalVersion : BaseEntity
{
    public Guid ProcesVerbalId { get; set; }
    public int NumeroVersion { get; set; }  
    public string Contenu { get; set; } = null!;
    public string UrlPdf { get; set; } = null!;
    public bool EstOfficielle { get; set; }
    public DateTime DateGeneration { get; set; }
    public Guid GenereParId { get; set; }

    public ProcesVerbal ProcesVerbal { get; set; } = null!;
}
