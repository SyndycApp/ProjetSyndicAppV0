using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;

namespace SyndicApp.Infrastructure.Services.Personnel;

public class PlanningCalendarService : IPlanningCalendarService
{
    private readonly ApplicationDbContext _db;

    public PlanningCalendarService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<PlanningCalendarDto>> GetAsync(
        DateOnly from,
        DateOnly to,
        Guid? employeId = null,
        Guid? residenceId = null)
    {
        var query = _db.PlanningMissions
            .Include(m => m.Employe)
            .Include(m => m.Residence)
            .Where(m => m.Date >= from && m.Date <= to)
            .AsQueryable();

        if (employeId.HasValue)
            query = query.Where(m => m.EmployeId == employeId);

        if (residenceId.HasValue)
            query = query.Where(m => m.ResidenceId == residenceId);

        return await query
            .OrderBy(m => m.Date)
            .ThenBy(m => m.HeureDebut)
            .Select(m => new PlanningCalendarDto
            {
                Date = m.Date,
                MissionId = m.Id,
                EmployeId = m.EmployeId,
                EmployeNom = m.Employe.Nom + " " + m.Employe.Prenom,
                ResidenceId = m.ResidenceId,
                ResidenceNom = m.Residence.Nom,
                Mission = m.Mission,
                HeureDebut = m.HeureDebut,
                HeureFin = m.HeureFin,
                Statut = m.Statut
            })
            .ToListAsync();
    }
}
