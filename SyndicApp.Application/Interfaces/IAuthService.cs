using SyndicApp.Application.DTOs.Auth;
using SyndicApp.Domain.Entities.Personnel;
using System;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UserDto>> RegisterAsync(RegisterDto dto);
        Task<Result<UserDto>> LoginAsync(LoginDto dto);
        Task<Result<UserDto>> GetByIdAsync(Guid userId);
        Task<Result<List<UserDto>>> GetAllAsync();

        Task<Result<List<UserLookupDto>>> SearchAsync(string? q, string? role, int take = 35 );

        Task<Prestataire?> GetPrestataireEntityAsync(Guid id);
        Task BindPrestataireToUserAsync(Guid prestataireId, Guid userId);

    }
}
