using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Identity
{
    public static class RolesSeeder
    {
        private static readonly string[] Roles = new[] { "Syndic", "Copropriétaire", "Gardien", "Locataire" };

        public static async Task SeedAsync(RoleManager<IdentityRole<Guid>> roleManager, ILogger logger)
        {
            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                    if (result.Succeeded)
                    {
                        logger.LogInformation($"? Rôle créé : {role}");
                    }
                    else
                    {
                        logger.LogError($"? Erreur lors de la création du rôle : {role}");
                    }
                }
                else
                {
                    logger.LogInformation($"?? Rôle déjà existant : {role}");
                }
            }
        }
    }
}
