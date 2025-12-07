using System.Security.Claims;

namespace SyndicApp.Infrastructure.Identity.Extensions
{
    public static class UserExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var id = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(id))
                throw new Exception("User ID claim manquant dans le token.");

            return Guid.Parse(id);
        }
    }
}
