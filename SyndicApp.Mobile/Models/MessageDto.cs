namespace SyndicApp.Mobile.Models
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using System;
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
        // 🔊 ÉTAT AUDIO (UI)
        // =====================
        [ObservableProperty] private bool isPlaying;
        [ObservableProperty] private double audioProgress;
        [ObservableProperty] private string audioTime = "00:00";

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
