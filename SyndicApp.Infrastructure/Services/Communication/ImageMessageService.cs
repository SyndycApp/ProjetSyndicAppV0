using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Communication;
using SyndicApp.Application.Interfaces.Communication;
using SyndicApp.Domain.Entities.Communication;
using SyndicApp.Infrastructure;
using SyndicApp.Infrastructure.Data;


namespace SyndicApp.Infrastructure.Services.Communication
{
    public class ImageMessageService : IImageMessageService
    {
        private readonly ApplicationDbContext _db;
        private readonly IFileStorageService _fileStorage;

        public ImageMessageService(
            ApplicationDbContext db,
            IFileStorageService fileStorage)
        {
            _db = db;
            _fileStorage = fileStorage;
        }

        public async Task<MessageDto> SendImageAsync(
            Guid userId,
            Guid conversationId,
            Stream imageStream,
            string fileName,
            string contentType)
        {
            var imageUrl = await _fileStorage.SaveAsync(
                imageStream,
                fileName,
                folder: "images",
                contentType
            );

            var message = new Message
            {
                ConversationId = conversationId,
                UserId = userId,
                Type = MessageType.Image,
                FileUrl = imageUrl,
                ContentType = contentType
            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            return message.ToDto();
        }
    }
}
