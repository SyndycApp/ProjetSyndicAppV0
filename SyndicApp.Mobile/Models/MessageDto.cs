namespace SyndicApp.Mobile.Models
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using System;
    using System.Collections.ObjectModel;
    using System.Text.Json.Serialization;

    public partial class MessageDto : ObservableObject
    {
        // =====================
        // 🔑 IDENTITÉ
        // =====================
        public Guid Id { get; set; }

        [JsonPropertyName("conversationId")]
        public Guid ConversationId { get; set; }

        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        // =====================
        // 📝 TEXTE
        // =====================
        [JsonPropertyName("contenu")]
        public string? Contenu { get; set; }

        // =====================
        // 🔊 AUDIO
        // =====================
        [JsonPropertyName("audioUrl")]
        public string? AudioUrl { get; set; }

        // =====================
        // 📎 FICHIER (IMAGE / DOC)
        // =====================
        [JsonPropertyName("fileUrl")]
        public string? FileUrl { get; set; }

        [JsonPropertyName("fileName")]
        public string? FileName { get; set; }

        [JsonPropertyName("contentType")]
        public string? ContentType { get; set; }

        // =====================
        // 📍 LOCALISATION
        // =====================
        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }

        // =====================
        // META
        // =====================
        [ObservableProperty]
        [property: JsonPropertyName("type")]
        private string type = "Text";

        [JsonPropertyName("nomExpediteur")]
        public string NomExpediteur { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("isRead")]
        public bool IsRead { get; set; }

        [JsonPropertyName("readAt")]
        public DateTime? ReadAt { get; set; }

        // =====================
        // 🎯 HELPERS TYPE (ROBUSTES)
        // =====================
        [JsonIgnore]
        public bool IsText =>
            Type?.Equals("Text", StringComparison.OrdinalIgnoreCase) == true
            && !string.IsNullOrWhiteSpace(Contenu);

        [JsonIgnore]
        public bool IsAudio =>
            Type?.Equals("Audio", StringComparison.OrdinalIgnoreCase) == true
            && !string.IsNullOrWhiteSpace(AudioUrl);

        [JsonIgnore]
        public bool IsImage =>
            Type?.Equals("Image", StringComparison.OrdinalIgnoreCase) == true
            && !string.IsNullOrWhiteSpace(FileUrl)
            && ContentType?.StartsWith("image/", StringComparison.OrdinalIgnoreCase) == true;

        [JsonIgnore]
        public bool IsDocument =>
            Type?.Equals("Document", StringComparison.OrdinalIgnoreCase) == true
            && !string.IsNullOrWhiteSpace(FileUrl)
            && ContentType != null
            && !ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);

        [JsonIgnore]
        public bool IsLocation =>
            Type?.Equals("Location", StringComparison.OrdinalIgnoreCase) == true
            && Latitude.HasValue
            && Longitude.HasValue;

        // =====================
        // 🌐 URL ABSOLUES
        // =====================
        [JsonIgnore]
        public string AbsoluteAudioUrl =>
            string.IsNullOrWhiteSpace(AudioUrl)
                ? string.Empty
                : AudioUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                    ? AudioUrl
                    : $"http://192.168.31.157:5041{AudioUrl}";

        [JsonIgnore]
        public string AbsoluteFileUrl =>
            string.IsNullOrWhiteSpace(FileUrl)
                ? string.Empty
                : FileUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                    ? FileUrl
                    : $"http://192.168.31.157:5041{FileUrl}";

        // =====================
        // 🔊 WAVE AUDIO (UI)
        // =====================
        [JsonIgnore]
        public ObservableCollection<double> Waveform { get; } =
              new ObservableCollection<double>(
              Enumerable.Range(0, 25).Select(i => (double)i)
        );

        [JsonIgnore] public int WaveBarCount => Waveform.Count;

        // =====================
        // 🔊 ÉTAT AUDIO (UI)
        // =====================
        [ObservableProperty] private bool isPlaying;
        [ObservableProperty] private double audioProgress;
        [ObservableProperty] private string audioTime = "00:00";

        [JsonIgnore]
        public string StaticMapTileUrl
        {
            get
            {
                if (!IsLocation) return string.Empty;

                const int zoom = 16;

                var latRad = Latitude!.Value * Math.PI / 180;
                var n = Math.Pow(2, zoom);

                var xTile = (int)((Longitude!.Value + 180.0) / 360.0 * n);
                var yTile = (int)((1.0 - Math.Log(Math.Tan(latRad) + 1 / Math.Cos(latRad)) / Math.PI) / 2.0 * n);

                return $"https://tile.openstreetmap.org/{zoom}/{xTile}/{yTile}.png";
            }
        }

        [JsonIgnore]
        public string StaticMapUrl
        {
            get
            {
                if (!IsLocation || Latitude == null || Longitude == null)
                    return string.Empty;

                var lat = Latitude.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
                var lng = Longitude.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);

                return
                    $"https://api.mapbox.com/styles/v1/mapbox/streets-v11/static/" +
                    $"pin-s+2563EB({lng},{lat})/" +
                    $"{lng},{lat},16/400x200" +
                    $"?access_token=pk.eyJ1Ijoic3luZGljIiwiYSI6ImNtajdwMnVqcjA1cDMzZnNmdXdjazcxZnEifQ.SW3kQZqj_8ypQDi10Mq-rQ";
            }
        }


        [JsonIgnore]
        public string ExternalMapUrl =>
    IsLocation
        ? $"https://www.google.com/maps/search/?api=1&query={Latitude},{Longitude}"
        : string.Empty;

        // =====================
        // 🔁 FORCE RAFRAÎCHISSEMENT UI
        // =====================
        partial void OnTypeChanged(string value)
        {
            OnPropertyChanged(nameof(IsText));
            OnPropertyChanged(nameof(IsAudio));
            OnPropertyChanged(nameof(IsImage));
            OnPropertyChanged(nameof(IsDocument));
            OnPropertyChanged(nameof(IsLocation));
        }
    }

    // =====================
    // DTO CREATION TEXTE
    // =====================
    public class CreateMessageDto
    {
        public Guid ConversationId { get; set; }
        public string Contenu { get; set; } = string.Empty;
    }
}
