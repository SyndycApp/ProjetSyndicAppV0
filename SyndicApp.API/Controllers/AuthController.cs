using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Auth;
using SyndicApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger = null!;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Si pas de refresh token, juste retourner OK.
            return Ok(new { message = "D�connexion r�ussie" });
        }

        /// <summary>
        /// Enregistre un nouvel utilisateur (Syndic, Copropri�taire, etc.)
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { message = "Validation �chou�e", details = errors });
            }

            _logger.LogInformation("API Register called");
            _logger.LogInformation("RegisterDto re�u : {@RegisterDto}", registerDto);

            if (registerDto == null)
            {
                return BadRequest(new { message = "Donn�es manquantes" });
            }

            var result = await _authService.RegisterAsync(registerDto);

            if (!result.Success)
            {
                List<string> errors;

                if (result.Errors == null)
                {
                    errors = new List<string> { "Erreur inconnue" };
                }
                else if (result.Errors is List<string> errorList)
                {
                    errors = errorList;
                }
                else
                {
                    errors = new List<string> { result.Errors.ToString() ?? "Erreur inconnue" };
                }

                return BadRequest(new { message = "�chec de l'inscription", details = errors });
            }

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
        /// R�cup�re les infos de l'utilisateur connect�
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            if (userId == null)
                return Unauthorized("Utilisateur non authentifi�");

            var user = await _authService.GetByIdAsync(Guid.Parse(userId));

            if (user == null)
                return NotFound("Utilisateur introuvable");

            return Ok(user);
        }
    }
}
