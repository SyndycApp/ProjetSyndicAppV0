namespace SyndicApp.Mobile.Models
{
    public class ConversationDto
    {
        public Guid Id { get; set; }
        public string Sujet { get; set; }
        public DateTime DateCreation { get; set; }

        public List<ParticipantDto> Participants { get; set; }

        public MessageDto DernierMessage { get; set; }

        public ParticipantDto OtherParticipant
        {
            get
            {
                try
                {
                    if (Participants == null || Participants.Count == 0)
                        return new ParticipantDto { NomComplet = "Inconnu" };

                    if (string.IsNullOrEmpty(App.UserId))
                        return Participants.First();

                    var myId = Guid.Parse(App.UserId);
                    var other = Participants.FirstOrDefault(p => p.UserId != myId);

                    return other ?? Participants.First();
                }
                catch
                {
                    return new ParticipantDto { NomComplet = "Erreur" };
                }
            }
        }

    }

    public class ParticipantDto
    {
        public Guid UserId { get; set; }
        public string NomComplet { get; set; }
    }
}
