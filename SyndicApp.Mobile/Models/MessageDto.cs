namespace SyndicApp.Mobile.Models
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using System;
    using System.Text.Json.Serialization;

    public partial class MessageDto : ObservableObject
    {
        public Guid Id { get; set; }

        [JsonPropertyName("conversationId")]
        public Guid ConversationId { get; set; }

        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

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
        [JsonPropertyName("type")]
        public string Type { get; set; } = "Text";

        [JsonPropertyName("nomExpediteur")]
        public string NomExpediteur { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("isRead")]
        public bool IsRead { get; set; }

        [JsonPropertyName("readAt")]
        public DateTime? ReadAt { get; set; }

        // =====================
        // 🎯 HELPERS TYPE (SÉCURISÉS)
        // =====================
        [JsonIgnore]
        public bool IsText =>
            Type == "Text" && !string.IsNullOrWhiteSpace(Contenu);

        [JsonIgnore]
        public bool IsAudio =>
            Type == "Audio" && !string.IsNullOrWhiteSpace(AudioUrl);

        [JsonIgnore]
        public bool IsImage =>
            Type == "Image"
            && !string.IsNullOrWhiteSpace(FileUrl)
            && ContentType != null
            && ContentType.StartsWith("image/");

        [JsonIgnore]
        public bool IsDocument =>
            Type == "Document"
            && !string.IsNullOrWhiteSpace(FileUrl)
            && ContentType != null
            && !ContentType.StartsWith("image/");

        [JsonIgnore]
        public bool IsLocation =>
            Type == "Location"
            && Latitude.HasValue
            && Longitude.HasValue;

        // =====================
        // 🌐 URL ABSOLUES
        // =====================
        [JsonIgnore]
        public string AbsoluteAudioUrl =>
            string.IsNullOrWhiteSpace(AudioUrl)
                ? string.Empty
                : AudioUrl.StartsWith("http")
                    ? AudioUrl
                    : $"http://192.168.1.200:5041{AudioUrl}";

        [JsonIgnore]
        public string AbsoluteFileUrl =>
            string.IsNullOrWhiteSpace(FileUrl)
                ? string.Empty
                : FileUrl.StartsWith("http")
                    ? FileUrl
                    : $"http://192.168.1.200:5041{FileUrl}";

        // =====================
        // 🔊 ÉTAT AUDIO (INTACT)
        // =====================
        [ObservableProperty] private bool isPlaying;
        [ObservableProperty] private double audioProgress;
        [ObservableProperty] private string audioTime = "00:00";
    }

    public class CreateMessageDto
    {
        public Guid ConversationId { get; set; }
        public string Contenu { get; set; } = string.Empty;
    }
}
