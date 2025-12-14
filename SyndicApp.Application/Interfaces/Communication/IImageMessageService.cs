using System;
using System.IO;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Communication;

namespace SyndicApp.Application.Interfaces.Communication
{
    public interface IImageMessageService
    {
        Task<MessageDto> SendImageAsync(
            Guid userId,
            Guid conversationId,
            Stream imageStream,
            string fileName,
            string contentType
        );
    }
}
