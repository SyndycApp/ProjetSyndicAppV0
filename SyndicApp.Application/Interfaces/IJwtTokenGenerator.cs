using System.Collections.Generic;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Auth;

namespace SyndicApp.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(UserDto user, IList<string> roles);
    }
}
