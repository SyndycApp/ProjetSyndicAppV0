using SyndicApp.Application.DTOs.Auth;
using System;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces
{
    public interface IPasswordService
    {
        Task<bool> GenerateResetTokenAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordDto model);
        Task<bool> SendResetCodeAsync(string email);
        Task<bool> VerifyResetCodeAsync(string email, string code);
        Task<bool> ResetWithCodeAsync(string email, string code, string newPassword, string confirmPassword);
    }
}