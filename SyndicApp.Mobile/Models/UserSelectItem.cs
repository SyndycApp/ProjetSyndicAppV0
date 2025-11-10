namespace SyndicApp.Mobile.Models
{    public sealed class UserSelectItem
    {
        public Guid Id { get; set; }                 
        public string Label { get; set; } = "";     
        public string? Email { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
