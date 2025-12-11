using SyndicApp.Application.DTOs.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Communication
{
    public interface IMessageService
    {
        Task<List<MessageDto>> GetMessagesAsync(Guid conversationId, Guid userId);
        Task MarkMessagesAsReadAsync(Guid conversationId, Guid userId);

        Task<PagedMessagesDto> GetMessagesPaged(Guid conversationId, int page, int pageSize);
        Task<MessageDto> SendMessageAsync(
            Guid userId,
            SendMessageRequest request
        );
    }
}
