using SyndicApp.Domain.Entities.Communication;

public class MessageReaction
{
    public Guid MessageId { get; set; }
    public Message Message { get; set; } = null!;

    public Guid UserId { get; set; }
    public string Emoji { get; set; } = string.Empty;
}
