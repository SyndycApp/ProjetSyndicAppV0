using SyndicApp.Domain.Entities.Common;
namespace SyndicApp.Domain.Entities.Assemblees;
public class SignatureProcesVerbal : BaseEntity
{
    public Guid ProcesVerbalId { get; set; }
    public ProcesVerbal ProcesVerbal { get; set; } = null!;
    public Guid UserId { get; set; }
    public int OrdreSignature { get; set; } 
    public bool EstObligatoire { get; set; } = true;
    public bool EstSigne { get; set; }
    public DateTime? DateSignature { get; set; }
    public string? Commentaire { get; set; }
}
