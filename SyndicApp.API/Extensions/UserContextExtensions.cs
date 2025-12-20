using System.Security.Claims;

namespace SyndicApp.API.Extensions
{
    public static class UserContextExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var id = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(id))
                throw new UnauthorizedAccessException("UserId not found in token");

            return Guid.Parse(id);
        }
    }
}
