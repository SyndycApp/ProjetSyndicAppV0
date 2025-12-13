using SyndicApp.Application.Interfaces.Communication;

namespace SyndicApp.Infrastructure.Files
{
    public class LocalAudioStorage : IAudioStorage
    {
        private readonly string _basePath = "wwwroot/uploads/audio";

        public async Task<string> SaveAsync(
            Stream audioStream,
            string fileName,
            string contentType)
        {
            Directory.CreateDirectory(_basePath);

            var uniqueName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(_basePath, uniqueName);

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await audioStream.CopyToAsync(fileStream);

            return $"/uploads/audio/{uniqueName}";
        }
    }
}
