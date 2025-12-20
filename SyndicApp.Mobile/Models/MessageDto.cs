namespace SyndicApp.Mobile.Models
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text.Json.Serialization;
    using SyndicApp.Mobile.Config;

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
        // 🎯 HELPERS TYPE
        // =====================
        [JsonIgnore] public bool IsText => Type == "Text" && !string.IsNullOrWhiteSpace(Contenu);
        [JsonIgnore] public bool IsAudio => Type == "Audio" && !string.IsNullOrWhiteSpace(AudioUrl);
        [JsonIgnore]
        public bool IsImage =>
            Type == "Image" &&
            !string.IsNullOrWhiteSpace(FileUrl) &&
            ContentType?.StartsWith("image/") == true;

        [JsonIgnore]
        public bool IsDocument =>
            Type == "Document" &&
            !string.IsNullOrWhiteSpace(FileUrl) &&
            ContentType != null &&
            !ContentType.StartsWith("image/");

        [JsonIgnore]
        public bool IsLocation =>
            Type == "Location" &&
            Latitude.HasValue &&
            Longitude.HasValue;

        // =====================
        // 🌐 URL ABSOLUES
        // =====================
        [JsonIgnore]
        public string AbsoluteAudioUrl =>
            string.IsNullOrWhiteSpace(AudioUrl)
                ? string.Empty
                : AudioUrl.StartsWith("http")
                    ? AudioUrl
                    : $"{AppConfig.ApiBaseUrl}{AudioUrl}";

        [JsonIgnore]
        public string AbsoluteFileUrl =>
            string.IsNullOrWhiteSpace(FileUrl)
                ? string.Empty
                : FileUrl.StartsWith("http")
                    ? FileUrl
                    : $"{AppConfig.ApiBaseUrl}{FileUrl}";

        // =====================
        // 🔊 AUDIO UI
        // =====================
        [ObservableProperty] private bool isPlaying;
        [ObservableProperty] private double audioProgress;
        [ObservableProperty] private string audioTime = "00:00";

        // =====================
        // 🔁 RÉPONSE
        // =====================
        [JsonPropertyName("replyToMessage")]
        public MessageDto? ReplyToMessage { get; set; }

        // =====================
        // 👍 RÉACTIONS (BRUTES)
        // =====================
        private ObservableCollection<MessageReactionDto> _reactions = new();

        [JsonPropertyName("reactions")]
        public ObservableCollection<MessageReactionDto> Reactions
        {
            get => _reactions;
            set
            {
                if (_reactions == value)
                    return;

                if (_reactions != null)
                    _reactions.CollectionChanged -= OnReactionsChanged;

                _reactions = value ?? new ObservableCollection<MessageReactionDto>();
                _reactions.CollectionChanged += OnReactionsChanged;

                RebuildGroupedReactions();
                OnPropertyChanged(nameof(HasGroupedReactions));
            }
        }

        private void OnReactionsChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RebuildGroupedReactions();
            OnPropertyChanged(nameof(HasGroupedReactions));
        }


        // =====================
        // 👍 RÉACTIONS GROUPÉES (UI)
        // =====================
        [JsonIgnore]
        public ObservableCollection<MessageReactionGroupDto> GroupedReactions { get; } = new();

        [JsonIgnore]
        public bool HasGroupedReactions => GroupedReactions.Count > 0;

        // =====================
        // 🔄 CONSTRUCTEUR (IMPORTANT)
        // =====================
        public MessageDto()
        {
            Reactions.CollectionChanged += (_, __) =>
            {
                RebuildGroupedReactions();
                OnPropertyChanged(nameof(HasGroupedReactions));
            };
        }

        // =====================
        // 🔁 REGROUPEMENT
        // =====================
        private void RebuildGroupedReactions()
        {
            GroupedReactions.Clear();

            if (Reactions.Count == 0)
                return;

            var groups = Reactions
                .GroupBy(r => r.Emoji)
                .Select(g => new MessageReactionGroupDto
                {
                    Emoji = g.Key,
                    Count = g.Count()
                });

            foreach (var g in groups)
                GroupedReactions.Add(g);
        }

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
}
