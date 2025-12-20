using SyndicApp.Domain.Enums;

namespace SyndicApp.Application.DTOs.AppelVocal
{
    public class CallDto
    {
        public Guid Id { get; set; }
        public Guid CallerId { get; set; }
        public Guid ReceiverId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public int? DurationSeconds { get; set; }
        public CallStatus Status { get; set; }
    }
}
