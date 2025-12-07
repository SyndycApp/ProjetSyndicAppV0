using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Communication;

namespace SyndicApp.Application.Interfaces.Communication
{
    public interface IConversationService
    {
        Task<List<ConversationDto>> GetUserConversationsAsync(Guid userId);

        Task<ConversationDto> CreateConversationAsync(
            Guid creatorId,
            CreateConversationRequest request
        );
    }
}
