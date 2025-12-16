namespace SyndicApp.Mobile.Models
{
    public class MessageReactionDto
    {
        public Guid UserId { get; set; }
        public string Emoji { get; set; } = string.Empty;
    }
}
