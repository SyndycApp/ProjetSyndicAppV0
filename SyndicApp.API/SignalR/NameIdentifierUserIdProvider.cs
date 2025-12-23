using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace SyndicApp.API.SignalR
{
    public class NameIdentifierUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            var claims = connection.User?.Claims;

            Console.WriteLine("🔎 Claims SignalR :");
            foreach (var c in claims!)
                Console.WriteLine($" - {c.Type} = {c.Value}");

            var userId =
                connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? connection.User?.FindFirst("sub")?.Value
                ?? connection.User?.FindFirst("uid")?.Value;

            Console.WriteLine($"🔑 SignalR UserId FINAL = {userId}");
            return userId;
        }
    }
}
