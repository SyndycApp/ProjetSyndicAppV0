namespace SyndicApp.Mobile.Models
{
    using System;
    using System.Text.Json.Serialization;


    public class MessageDto
    {
        public Guid Id { get; set; }

        [JsonPropertyName("conversationId")]
        public Guid ConversationId { get; set; }

        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }   

        [JsonPropertyName("contenu")]
        public string Contenu { get; set; }

        [JsonPropertyName("nomExpediteur")]
        public string NomExpediteur { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("isRead")]
        public bool IsRead { get; set; }

        [JsonPropertyName("readAt")]
        public DateTime? ReadAt { get; set; }
    }


    public class CreateMessageDto
    {
        public Guid ConversationId { get; set; }
        public string Contenu { get; set; } = string.Empty;
    }

}
