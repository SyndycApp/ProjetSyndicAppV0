using SyndicApp.Domain.Entities.Users;
using System;

namespace SyndicApp.Domain.Entities.Common;

public class Notification : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    // Relations
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}