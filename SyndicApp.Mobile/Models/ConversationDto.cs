namespace SyndicApp.Mobile.Models
{
    public class ConversationDto
    {
        public Guid Id { get; set; }
        public string Sujet { get; set; }
        public DateTime DateCreation { get; set; }

        public List<ParticipantDto> Participants { get; set; }

        public MessageDto DernierMessage { get; set; }
    }

    public class ParticipantDto
    {
        public Guid UserId { get; set; }
        public string NomComplet { get; set; }
    }
}
