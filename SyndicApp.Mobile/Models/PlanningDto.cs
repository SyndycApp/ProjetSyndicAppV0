using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.Models
{
    public class PlanningDto
    {
        public string Jour { get; set; } = string.Empty;
        public string HeureDebut { get; set; } = string.Empty;
        public string HeureFin { get; set; } = string.Empty;
    }

    public class PresenceDto
    {
        public DateTime Date { get; set; }
        public string? HeureArrivee { get; set; }
        public string? HeureDepart { get; set; }
    }
}
