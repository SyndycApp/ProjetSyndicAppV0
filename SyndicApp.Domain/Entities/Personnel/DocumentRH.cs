using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Personnel;

public class DocumentRH : BaseEntity
{
    public Guid EmployeId { get; set; }
    public Employe Employe { get; set; } = null!;
    public string Type { get; set; } = string.Empty; 
    public string FilePath { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
}
