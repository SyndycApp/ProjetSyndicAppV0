using SyndicApp.Domain.Entities.Common;
using System;

namespace SyndicApp.Domain.Entities.Incidents
{
    public class DevisHistorique : BaseEntity
    {
        public Guid DevisTravauxId { get; set; }
        public DevisTravaux DevisTravaux { get; set; } = null!;

        public DateTime DateAction { get; set; } = DateTime.UtcNow;
        public string Action { get; set; } = string.Empty;     
        public string? Commentaire { get; set; }

        public Guid AuteurId { get; set; }                     
    }
}
