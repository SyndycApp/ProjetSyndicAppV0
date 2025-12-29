using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Personnel
{
    public class PersonnelStatsDto
    {
        public double HeuresTravaillees { get; set; }
        public int TauxPresence { get; set; }
        public int ScoreFiabilite { get; set; }
    }
}
