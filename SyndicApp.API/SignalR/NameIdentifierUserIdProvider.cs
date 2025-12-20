using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace SyndicApp.API.SignalR
{
    public class NameIdentifierUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;
        }
    }
}
