// SyndicApp.Application/DTOs/Auth/UserLookupDto.cs
namespace SyndicApp.Application.DTOs.Auth
{
    public class UserLookupDto
    {
        public Guid Id { get; set; }
        public string Label { get; set; } = string.Empty; // FullName ou Email
        public string? Email { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
