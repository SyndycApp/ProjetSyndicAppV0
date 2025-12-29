using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Domain.Entities.Personnel;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.Infrastructure;
using SyndicApp.Infrastructure.Identity;


public class EmployeService : IEmployeService
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public EmployeService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }


    public async Task UpdateEmployeAsync(Guid userId, EmployeUpdateDto dto)
    {
        var profil = await _db.EmployeProfils
            .Include(p => p.Horaires)
            .Include(p => p.Missions)
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profil == null)
        {
            profil = new EmployeProfil
            {
                UserId = userId
            };
            _db.EmployeProfils.Add(profil);
        }

        profil.TypeContrat = dto.TypeContrat ?? string.Empty;
        profil.DateDebut = dto.DateDebutContrat ?? DateTime.MinValue;
        profil.DateFin = dto.DateFinContrat;

        profil.Horaires.Clear();
        foreach (var h in dto.Horaires)
        {
            var jourValide = Enum.TryParse<DayOfWeek>(h.Jour, true, out var day)
                ? day
                : DayOfWeek.Monday;

            profil.Horaires.Add(new HoraireTravail
            {
                Jour = jourValide,
                HeureDebut = h.HeureDebut,
                HeureFin = h.HeureFin
            });
        }

        profil.Missions.Clear();
        profil.Missions = dto.Missions.Select(m => new MissionEmploye
        {
            Libelle = m
        }).ToList();

        await _db.SaveChangesAsync();
    }

    public async Task<EmployeDetailsDto> GetEmployeDetailsAsync(Guid userId)
    {
        // 1️⃣ Utilisateur + rôle
        var user = await _userManager.Users
            .FirstAsync(u => u.Id == userId);

        var role = (await _userManager.GetRolesAsync(user))
            .FirstOrDefault() ?? string.Empty;

        // 2️⃣ Profil employé AVEC relations
        var profil = await _db.EmployeProfils
            .Include(p => p.Horaires)
            .Include(p => p.Missions)
            .FirstOrDefaultAsync(p => p.UserId == userId);

        // 3️⃣ Résidences affectées (via lots)
        var residences = await _db.AffectationsLots
            .Where(a => a.UserId == userId)
            .Select(a => a.Lot.Residence.Nom)
            .Distinct()
            .ToListAsync();

        // 4️⃣ Mapping DTO FINAL
        return new EmployeDetailsDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email!,
            Role = role,

            // ===== CONTRAT =====
            TypeContrat = profil?.TypeContrat ?? "Non défini",
            DateDebutContrat = profil?.DateDebut ?? DateTime.MinValue,
            DateFinContrat = profil?.DateFin,

            // ===== HORAIRES =====
            Horaires = profil?.Horaires
                .Select(h => new HoraireDto
                {
                    Jour = h.Jour.ToString(),
                    HeureDebut = h.HeureDebut,
                    HeureFin = h.HeureFin
                })
                .ToList() ?? new List<HoraireDto>(),

            // ===== MISSIONS =====
            Missions = profil?.Missions
                .Select(m => m.Libelle)
                .ToList() ?? new List<string>(),

            // ===== RÉSIDENCES =====
            Residences = residences
        };
    }
}
