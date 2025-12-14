using System;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Communication;

namespace SyndicApp.Application.Interfaces.Communication
{
    public interface ILocationMessageService
    {
        Task<MessageDto> SendLocationAsync(
            Guid userId,
            Guid conversationId,
            double latitude,
            double longitude
        );
    }
}
