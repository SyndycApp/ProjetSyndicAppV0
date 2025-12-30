using System;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Personnel
{
    public interface IAbsenceDocumentService
    {
        Task UploadAsync(
            Guid justificationId,
            Guid uploadedByUserId,
            string fileName,
            byte[] content);

        Task<(byte[] Content, string FileName)> DownloadAsync(Guid justificationId);
    }
}
