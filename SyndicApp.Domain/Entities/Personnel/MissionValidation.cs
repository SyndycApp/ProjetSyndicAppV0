using SyndicApp.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Domain.Entities.Personnel
{
    public class MissionValidation : BaseEntity
    {
        public Guid PlanningMissionId { get; set; }
        public PlanningMission PlanningMission { get; set; } = null!;

        public bool EstValidee { get; set; }
        public DateTime? DateValidation { get; set; }
        public string? Commentaire { get; set; }
    }

}
