namespace SyndicApp.Application.DTOs.Personnel;
public class PlanningJourDto
{
    public DateOnly Date { get; set; }
    public List<PlanningMissionDto> Missions { get; set; } = new();
}
