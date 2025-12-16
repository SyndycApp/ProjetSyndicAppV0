using SyndicApp.Application.DTOs.Communication;


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
        Task<MessageDto> SendAudioMessageAsync(
           Guid userId,
           Guid conversationId,
           Stream audioStream,
           string fileName,
           string contentType);

        Task AddReactionAsync(Guid messageId, Guid userId, string emoji);

    }
}
