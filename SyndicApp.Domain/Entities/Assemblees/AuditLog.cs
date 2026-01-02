using SyndicApp.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Domain.Entities.Assemblees
{
    public class AuditLog : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Action { get; set; } = null!;
        public string Cible { get; set; } = null!;
        public DateTime DateAction { get; set; }
    }
}
