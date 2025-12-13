using Plugin.Maui.Audio;

namespace SyndicApp.Mobile.Services.Communication
{
    public class AudioRecorderService
    {
        private readonly IAudioRecorder _recorder;

        public AudioRecorderService(IAudioManager audioManager)
        {
            _recorder = audioManager.CreateRecorder();
        }

        public async Task StartAsync()
        {
            await _recorder.StartAsync();
        }

        public async Task<string> StopAsync()
        {
            // ✅ API stable : StopAsync retourne une source audio
            var audioSource = await _recorder.StopAsync();

            if (audioSource == null)
                throw new InvalidOperationException("Aucun audio enregistré");

            var filePath = Path.Combine(
                FileSystem.CacheDirectory,
                $"audio_{DateTime.UtcNow.Ticks}.wav");

            // Sauvegarde manuelle du stream
            using var sourceStream = audioSource.GetAudioStream();
            using var fileStream = File.Create(filePath);
            await sourceStream.CopyToAsync(fileStream);

            return filePath;
        }
    }
}
