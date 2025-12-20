using SyndicApp.Application.DTOs.Communication;
using SyndicApp.Application.Interfaces.Communication;
using SyndicApp.Domain.Entities.Communication;
using SyndicApp.Infrastructure;
using SyndicApp.Infrastructure.Data;


namespace SyndicApp.Infrastructure.Services.Communication
{
    public class LocationMessageService : ILocationMessageService
    {
        private readonly ApplicationDbContext _db;

        public LocationMessageService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<MessageDto> SendLocationAsync(
            Guid userId,
            Guid conversationId,
            double latitude,
            double longitude)
        {
            var message = new Message
            {
                ConversationId = conversationId,
                UserId = userId,
                Type = MessageType.Location,
                Latitude = latitude,
                Longitude = longitude
            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            return message.ToDto();
        }
    }
}
