namespace SyndicApp.Mobile.Models;

public sealed class UserDto
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public List<string>? Roles { get; set; }   // <= Liste  "Syndic" | "Coproprietaire" | "Gardien" | "Locataire"
    public string? Token { get; set; }     
}
