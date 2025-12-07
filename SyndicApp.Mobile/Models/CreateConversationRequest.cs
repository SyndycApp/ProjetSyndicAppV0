namespace SyndicApp.Mobile.Models
{
    public class CreateConversationRequest
    {
        public string Sujet { get; set; } = string.Empty;
        public List<Guid> ParticipantsIds { get; set; } = new();
    }
}
