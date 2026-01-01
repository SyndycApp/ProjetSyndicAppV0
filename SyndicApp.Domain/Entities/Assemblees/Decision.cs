using SyndicApp.Domain.Entities.Common;
using System;

namespace SyndicApp.Domain.Entities.Assemblees
{
    public class Decision : BaseEntity
    {

        public Guid AssembleeGeneraleId { get; set; }
        public AssembleeGenerale AssembleeGenerale { get; set; } = null!;
        public Guid ResolutionId { get; set; }
        public Resolution Resolution { get; set; } = null!;

        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal TotalPour { get; set; }
        public decimal TotalContre { get; set; }
        public decimal TotalAbstention { get; set; }
        public decimal TotalExprime { get; set; }
        public bool EstAdoptee { get; set; }
        public DateTime DateDecision { get; set; }

        public Guid CreeParId { get; set; }        
        public bool EstVerrouillee { get; set; }   
    }
}
