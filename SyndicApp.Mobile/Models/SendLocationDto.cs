namespace SyndicApp.Mobile.Models
{
    public class SendLocationDto
    {
        public Guid ConversationId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
