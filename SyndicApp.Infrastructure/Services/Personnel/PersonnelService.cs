using Microsoft.AspNetCore.Identity;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.Infrastructure.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Services.Personnel
{
    public class PersonnelService : IPersonnelService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private static readonly string[] RolesEmployes =
        {
            "Gardien",
            "FemmeDeMenage"
        };

        public PersonnelService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IReadOnlyList<PersonnelLookupDto>> GetPersonnelInterneAsync()
        {
            var result = new List<PersonnelLookupDto>();

            foreach (var role in RolesEmployes)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role);

                foreach (var u in usersInRole)
                {
                    result.Add(new PersonnelLookupDto
                    {
                        UserId = u.Id,
                        FullName = u.FullName,
                        Email = u.Email ?? string.Empty,
                        Role = role
                    });
                }
            }

            // Sécurité : éviter doublons si un user a plusieurs rôles
            return result
                .GroupBy(x => x.UserId)
                .Select(g => g.First())
                .OrderBy(x => x.FullName)
                .ToList();
        }
    }
}
