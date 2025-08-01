using SyndicApp.Application.DTOs.Auth;
using System;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces
{
    public interface IPasswordService
    {
        Task<bool> GenerateResetTokenAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordDto model);
    }
}