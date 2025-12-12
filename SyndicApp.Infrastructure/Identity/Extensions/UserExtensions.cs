using System.Security.Claims;

namespace SyndicApp.Infrastructure.Identity.Extensions
{
    public static class UserExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            if (user == null)
                return Guid.Empty;
            string[] keys =
            {
                "uid",                          
                ClaimTypes.NameIdentifier,      
                "sub"                           
            };
            foreach (var key in keys)
            {
                var val = user.FindFirst(key)?.Value;
                if (!string.IsNullOrWhiteSpace(val) && Guid.TryParse(val, out var guid))
                    return guid;
            }

            return Guid.Empty;
        }
    }
}
