namespace SyndicApp.Mobile.Models
{
    public class UserChatDto
    {
        public Guid UserId { get; set; }
        public string NomComplet { get; set; }
    }
    public class OpenChatResponse
    {
        public Guid ConversationId { get; set; }
    }

    public class OpenChatRequest
    {
        public Guid OtherUserId { get; set; }
    }

}
