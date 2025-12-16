namespace SyndicApp.Mobile.Models;

public class ReactionCommandParam
{
    public MessageDto Message { get; set; } = null!;
    public string Emoji { get; set; } = string.Empty;
}
