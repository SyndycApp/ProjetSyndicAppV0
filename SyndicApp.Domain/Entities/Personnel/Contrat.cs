using System;
using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Personnel
{
    public class Contrat : BaseEntity
    {
        public Guid EmployeId { get; set; }
        public Employe Employe { get; set; } = null!;

        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }

        public string TypeContrat { get; set; } = string.Empty; // CDI, CDD...

        public string? FichierContratUrl { get; set; } // Lien vers le PDF signé
    }
}
