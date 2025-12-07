namespace SyndicApp.Mobile.Models
{
    public class SendMessageRequest
    {
        public Guid ConversationId { get; set; }
        public string Contenu { get; set; }
    }
}
