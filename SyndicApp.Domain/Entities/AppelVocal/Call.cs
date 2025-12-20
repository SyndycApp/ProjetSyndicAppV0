using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Enums;

namespace SyndicApp.Domain.Entities.AppelVocal
{
    public class Call : BaseEntity
    {
        public Guid CallerId { get; set; }
        public Guid ReceiverId { get; set; }

        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        public CallStatus Status { get; set; }

        public int? DurationSeconds =>
            EndedAt.HasValue
                ? (int)(EndedAt.Value - StartedAt).TotalSeconds
                : null;
    }
}
