using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Auth;
using SyndicApp.Application.Interfaces;


namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly IPasswordService _svc;

        public PasswordController(IPasswordService svc) => _svc = svc;


        [HttpPost("forgot-code")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotCode([FromBody] ForgotPasswordDto dto)
        {
            _ = await _svc.SendResetCodeAsync(dto.Email);
            return Ok(); // Toujours 200 pour éviter l’énumération d’emails
        }

        [HttpPost("verify-code")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyResetCodeDto dto)
        {
            var ok = await _svc.VerifyResetCodeAsync(dto.Email, dto.Code);
            return ok ? Ok() : Unauthorized();
        }

        [HttpPost("reset-with-code")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetWithCode([FromBody] ResetWithCodeDto dto)
        {
            var ok = await _svc.ResetWithCodeAsync(dto.Email, dto.Code, dto.NewPassword, dto.ConfirmPassword);
            return ok ? Ok() : BadRequest();
        }


        [HttpPost("forgot")]
        public async Task<IActionResult> Forgot([FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("L'adresse email est requise.");

            var result = await _svc.GenerateResetTokenAsync(dto.Email);

            if (!result)
                return NotFound("Aucun utilisateur trouvé avec cet email.");

            return Ok();
        }


        [HttpPost("reset")]
        public async Task<IActionResult> Reset([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var ok = await _svc.ResetPasswordAsync(dto);
            return ok ? Ok() : BadRequest();
        }
    }
}
