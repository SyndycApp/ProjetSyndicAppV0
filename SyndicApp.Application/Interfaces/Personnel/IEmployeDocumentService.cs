using SyndicApp.Application.DTOs.Personnel;

namespace SyndicApp.Application.Interfaces.Personnel
{
    public interface IEmployeDocumentService
    {
        Task UploadAsync(Guid userId, UploadEmployeDocumentDto dto);
        Task<IReadOnlyList<EmployeDocumentDto>> GetByEmployeAsync(Guid employeId);
        Task<(byte[] Content, string FileName)> DownloadAsync(Guid documentId);
    }
}
