using System;

namespace SyndicApp.Domain.Entities.Common;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = null!;
    public bool IsRead { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    public Guid? CibleId { get; set; }
    public string? CibleType { get; set; }
}