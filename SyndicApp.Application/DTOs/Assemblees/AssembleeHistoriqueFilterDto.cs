using SyndicApp.Domain.Enums.Assemblees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Assemblees
{
    public class AssembleeHistoriqueFilterDto
    {
        public int? Annee { get; set; }
        public StatutAssemblee? Statut { get; set; }
        public TypeAssemblee? Type { get; set; }
    }
}
