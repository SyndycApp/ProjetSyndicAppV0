using System;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SyndicApp.Application.DTOs.Auth;
using SyndicApp.Infrastructure;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly IPasswordService _svc;

        public PasswordController(IPasswordService svc) => _svc = svc;


        [HttpPost("forgot")]
        public async Task<IActionResult> Forgot([FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _svc.GenerateResetTokenAsync(dto.Email);
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
