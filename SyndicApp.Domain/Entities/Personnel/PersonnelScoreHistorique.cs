using SyndicApp.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Domain.Entities.Personnel
{
    public class PersonnelScoreHistorique : BaseEntity
    {
        public Guid EmployeId { get; set; }
        public int Annee { get; set; }
        public int Mois { get; set; }

        public double ScoreBrut { get; set; }
        public int ScoreNormalise { get; set; }
    }

}
