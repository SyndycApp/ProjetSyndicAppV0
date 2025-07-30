namespace SyndicApp.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }
}
