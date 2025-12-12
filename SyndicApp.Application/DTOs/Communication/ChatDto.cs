namespace SyndicApp.Application.DTOs.Communication
{
    public class UserChatDto
    {
        public Guid UserId { get; set; }
        public string NomComplet { get; set; } = string.Empty;
    }

    public class OpenChatRequest
    {
        public Guid OtherUserId { get; set; }
    }

    public class OpenChatResponse
    {
        public Guid ConversationId { get; set; }
    }
}
