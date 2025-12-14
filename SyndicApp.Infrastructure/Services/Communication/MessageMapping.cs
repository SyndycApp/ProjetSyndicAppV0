using SyndicApp.Domain.Entities.Communication;
using SyndicApp.Application.DTOs.Communication;

public static class MessageMapping
{
    public static MessageDto ToDto(this Message m)
    {
        return new MessageDto
        {
            Id = m.Id,
            ConversationId = m.ConversationId,
            UserId = m.UserId,

            Contenu = m.Contenu,

            IsRead = m.IsRead,
            ReadAt = m.ReadAt,
            CreatedAt = m.CreatedAt,

            Type = m.Type,

            // 🎧 AUDIO
            AudioUrl = m.Type == MessageType.Audio ? m.FileUrl : null,

            // 📸 IMAGE / 📄 DOCUMENT
            FileUrl = m.Type == MessageType.Image || m.Type == MessageType.Document
                        ? m.FileUrl
                        : null,

            FileName = m.Type == MessageType.Document ? m.FileName : null,
            ContentType = m.ContentType,

            // 📍 LOCATION
            Latitude = m.Latitude,
            Longitude = m.Longitude
        };
    }
}
