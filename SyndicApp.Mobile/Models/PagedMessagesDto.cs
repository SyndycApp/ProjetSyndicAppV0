namespace SyndicApp.Mobile.Models
{
    public class PagedMessagesDto
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<MessageDto> Messages { get; set; }
    }
}
