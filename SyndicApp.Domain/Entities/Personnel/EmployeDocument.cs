using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Enums;

namespace SyndicApp.Domain.Entities.Personnel;

public class EmployeDocument : BaseEntity
{
    public Guid EmployeId { get; set; }
    public Employe Employe { get; set; } = null!;

    public DocumentRHType Type { get; set; }

    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public Guid UploadedByUserId { get; set; }
}
