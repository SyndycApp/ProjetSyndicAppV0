namespace SyndicApp.Mobile.Models
{
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

        [JsonPropertyName("audioUrl")]
        public string? AudioUrl { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = "Text";

        [JsonPropertyName("nomExpediteur")]
        public string NomExpediteur { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("isRead")]
        public bool IsRead { get; set; }

        [JsonPropertyName("readAt")]
        public DateTime? ReadAt { get; set; }

        [JsonIgnore]
        public bool IsAudio => Type == "Audio" && !string.IsNullOrEmpty(AudioUrl);

        [JsonIgnore]
        public bool IsText => Type == "Text";

        [JsonIgnore]
        public string AbsoluteAudioUrl =>
            string.IsNullOrWhiteSpace(AudioUrl)
                ? string.Empty
                : AudioUrl.StartsWith("http")
                    ? AudioUrl
                    : $"http://192.168.1.200:5041{AudioUrl}";

        // 🔊 ÉTAT AUDIO PAR MESSAGE (clé du fix)
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
