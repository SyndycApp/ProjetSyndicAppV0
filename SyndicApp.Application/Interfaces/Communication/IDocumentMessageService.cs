using System;
using System.IO;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Communication;

namespace SyndicApp.Application.Interfaces.Communication
{
    public interface IDocumentMessageService
    {
        Task<MessageDto> SendDocumentAsync(
            Guid userId,
            Guid conversationId,
            Stream documentStream,
            string fileName,
            string contentType
        );
    }
}
