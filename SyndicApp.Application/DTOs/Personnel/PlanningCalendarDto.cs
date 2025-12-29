using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Personnel
{
    public class PlanningCalendarDto
    {
        public DateOnly Date { get; set; }
        public Guid MissionId { get; set; }

        public Guid EmployeId { get; set; }
        public string EmployeNom { get; set; } = string.Empty;

        public Guid ResidenceId { get; set; }
        public string ResidenceNom { get; set; } = string.Empty;

        public string Mission { get; set; } = string.Empty;

        public TimeSpan HeureDebut { get; set; }
        public TimeSpan HeureFin { get; set; }

        public string Statut { get; set; } = string.Empty;
    }
}
