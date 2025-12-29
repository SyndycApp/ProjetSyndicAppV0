using SyndicApp.Domain.Enums;

namespace SyndicApp.Application.DTOs.Personnel
{
    public record UploadEmployeDocumentDto(
        Guid EmployeId,
        DocumentRHType Type,
        string FileName,
        byte[] Content
    );

    public record EmployeDocumentDto(
    Guid Id,
    DocumentRHType Type,
    string FileName,
    long FileSize,
    DateTime CreatedAt
);

}
