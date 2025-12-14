namespace SyndicApp.API.Requests
{
    public class SendLocationRequest
    {
        public Guid ConversationId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
