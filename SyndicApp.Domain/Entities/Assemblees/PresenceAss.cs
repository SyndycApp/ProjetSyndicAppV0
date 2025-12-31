using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Enums.Assemblees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Domain.Entities.Assemblees
{
    public class PresenceAss : BaseEntity
    {
        public Guid AssembleeGeneraleId { get; set; }
        public AssembleeGenerale AssembleeGenerale { get; set; } = null!;

        public Guid UserId { get; set; }
        public Guid LotId { get; set; }

        public TypePresence Type { get; set; }

        public decimal Tantiemes { get; set; }
        public DateTime DatePresence { get; set; }
    }

}
