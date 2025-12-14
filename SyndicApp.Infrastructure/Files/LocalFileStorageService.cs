using SyndicApp.Application.Interfaces.Communication;

namespace SyndicApp.Infrastructure.Files
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _basePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "uploads"
        );

        public async Task<string> SaveAsync(
            Stream stream,
            string fileName,
            string folder,
            string contentType)
        {
            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);

            var folderPath = Path.Combine(_basePath, folder);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var safeFileName = $"{Guid.NewGuid()}_{fileName}";
            var fullPath = Path.Combine(folderPath, safeFileName);

            using var fileStream = new FileStream(fullPath, FileMode.Create);
            await stream.CopyToAsync(fileStream);

            // 🔗 URL exposée
            return $"/uploads/{folder}/{safeFileName}";
        }
    }
}
