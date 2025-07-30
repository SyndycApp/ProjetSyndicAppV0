using SyndicApp.Application.DTOs.Auth;
using System;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UserDto>> RegisterAsync(RegisterDto dto);
        Task<Result<UserDto>> LoginAsync(LoginDto dto);
        Task<Result<UserDto>> GetByIdAsync(Guid userId);
    }
}
