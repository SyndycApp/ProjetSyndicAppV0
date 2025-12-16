namespace SyndicApp.Mobile.Models
{
    public class SendMessageRequest
    {
        public Guid ConversationId { get; set; }

        public Guid? ReplyToMessageId { get; set; }
        public string Contenu { get; set; }
    }
}
