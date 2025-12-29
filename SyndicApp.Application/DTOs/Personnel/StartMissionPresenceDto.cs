using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Personnel
{
    public class StartMissionPresenceDto
    {
        public Guid PlanningMissionId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }


    public class EndMissionPresenceDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class PresenceMissionDto
    {
        public Guid MissionId { get; set; }
        public DateTime? HeureDebut { get; set; }
        public DateTime? HeureFin { get; set; }
        public bool IsGeoValidated { get; set; }
        public string? Anomalie { get; set; }
    }
}
