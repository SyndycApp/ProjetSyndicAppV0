using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Enums;

namespace SyndicApp.Domain.Entities.Personnel
{
    public class AbsenceJustification : BaseEntity
    {
        public Guid UserId { get; set; }
        public DateOnly Date { get; set; }

        public AbsenceType Type { get; set; }
        public string? Motif { get; set; }
        public string? DocumentUrl { get; set; }

        public bool Validee { get; set; }
    }
}
