using SyndicApp.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Domain.Entities.Personnel
{
    public class PrestataireNote : BaseEntity
    {
        public Guid PrestataireId { get; set; }
        public int Qualite { get; set; }
        public int Delai { get; set; }
        public int Communication { get; set; }

        public Guid AuteurSyndicId { get; set; }
    }
}
