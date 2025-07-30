namespace SyndicApp.Infrastructure.Identity
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = "SyndicApp";
        public string Audience { get; set; } = "SyndicAppUser";
        public int ExpiryMinutes { get; set; } = 60;
    }
}
