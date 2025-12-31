using SyndicApp.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Domain.Entities.Assemblees
{
    public class Procuration : BaseEntity
    {
        public Guid AssembleeGeneraleId { get; set; }

        public Guid DonneurId { get; set; }
        public Guid MandataireId { get; set; }

        public Guid LotId { get; set; }

        public DateTime DateCreation { get; set; }
    }

}
