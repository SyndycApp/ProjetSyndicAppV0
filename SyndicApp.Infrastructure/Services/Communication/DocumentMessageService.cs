using SyndicApp.Application.DTOs.Communication;
using SyndicApp.Application.Interfaces.Communication;
using SyndicApp.Domain.Entities.Communication;
using SyndicApp.Infrastructure;
using SyndicApp.Infrastructure.Data;


namespace SyndicApp.Infrastructure.Services.Communication
{
    public class DocumentMessageService : IDocumentMessageService
    {
        private readonly ApplicationDbContext _db;
        private readonly IFileStorageService _fileStorage;

        public DocumentMessageService(
            ApplicationDbContext db,
            IFileStorageService fileStorage)
        {
            _db = db;
            _fileStorage = fileStorage;
        }

        public async Task<MessageDto> SendDocumentAsync(
            Guid userId,
            Guid conversationId,
            Stream documentStream,
            string fileName,
            string contentType)
        {
            var documentUrl = await _fileStorage.SaveAsync(
                documentStream,
                fileName,
                folder: "documents",
                contentType
            );

            var message = new Message
            {
                ConversationId = conversationId,
                UserId = userId,
                Type = MessageType.Document,
                FileUrl = documentUrl,
                FileName = fileName,
                ContentType = contentType
            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            return message.ToDto();
        }
    }
}
