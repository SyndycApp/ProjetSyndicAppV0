using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Auth;
using SyndicApp.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace SyndicApp.API.Controllers
{
    [Authorize(Roles = "Syndic")]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Enregistre un nouvel utilisateur (Syndic, Copropriétaire, etc.)
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        /// <summary>
        /// Connecte un utilisateur existant
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            if (!result.Success)
                return Unauthorized(result.Errors);

            return Ok(result.Data);
        }

        /// <summary>
        /// Récupère les infos de l'utilisateur connecté
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            if (userId == null)
                return Unauthorized("Utilisateur non authentifié");

            var user = await _authService.GetByIdAsync(Guid.Parse(userId));

            if (user == null)
                return NotFound("Utilisateur introuvable");

            return Ok(user);
        }
    }
}
