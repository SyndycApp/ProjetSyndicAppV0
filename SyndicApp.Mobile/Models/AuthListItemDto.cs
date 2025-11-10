// SyndicApp.Mobile/Models/AuthListItemDto.cs
namespace SyndicApp.Mobile.Models
{
    public sealed class AuthListItemDto
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
