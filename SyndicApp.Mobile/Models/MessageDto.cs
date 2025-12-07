namespace SyndicApp.Mobile.Models
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Guid UserId { get; set; }
        public string NomExpediteur { get; set; }
        public string Contenu { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
