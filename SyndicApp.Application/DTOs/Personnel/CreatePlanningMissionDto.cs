using SyndicApp.Domain.Entities.Personnel;

namespace SyndicApp.Application.DTOs.Personnel
{
    public class CreatePlanningMissionDto
    {
        public Guid EmployeId { get; set; }
        public Guid ResidenceId { get; set; }

        public string Mission { get; set; } = string.Empty;

        public DateOnly Date { get; set; }
        public TimeSpan HeureDebut { get; set; }
        public TimeSpan HeureFin { get; set; }
    }

    public class UpdatePlanningMissionDto
    {
        public string Mission { get; set; } = string.Empty;

        public TimeSpan HeureDebut { get; set; }
        public TimeSpan HeureFin { get; set; }

        public string Statut { get; set; } = "Planifiee";
    }

    public class PlanningMissionDto
    {
        public Guid Id { get; set; }

        public Guid EmployeId { get; set; }
        public Guid ResidenceId { get; set; }

        public string Mission { get; set; } = string.Empty;

        public DateOnly Date { get; set; }
        public TimeSpan HeureDebut { get; set; }
        public TimeSpan HeureFin { get; set; }

        public string Statut { get; set; } = string.Empty;

        public PlanningMissionDto() { }

        public PlanningMissionDto(PlanningMission entity)
        {
            Id = entity.Id;
            EmployeId = entity.EmployeId;
            ResidenceId = entity.ResidenceId;
            Mission = entity.Mission;
            Date = entity.Date;
            HeureDebut = entity.HeureDebut;
            HeureFin = entity.HeureFin;
            Statut = entity.Statut;
        }
    }
}
