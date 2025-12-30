namespace SyndicApp.Application.DTOs.Personnel
{
    public record UploadAbsenceJustificatifDto
    (
        Guid JustificationId,
        string FileName,
        byte[] Content
    );
}
