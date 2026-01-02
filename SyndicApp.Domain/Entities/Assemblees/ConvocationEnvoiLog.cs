using SyndicApp.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Domain.Entities.Assemblees
{
    public class ConvocationEnvoiLog : BaseEntity
    {
        public Guid ConvocationId { get; set; }
        public Convocation Convocation { get; set; } = null!;

        public Guid UserId { get; set; }

        public string Email { get; set; } = null!;
        public DateTime DateEnvoi { get; set; }
        public bool Succes { get; set; }
        public string? Erreur { get; set; }
    }
}
