using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Auth;
using SyndicApp.Application.Interfaces;
using SyndicApp.Infrastructure.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Claims;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger = null!;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _logger = logger;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // uid (custom), sub (JWT standard) ou nameidentifier
            var id = User.FindFirst("uid")?.Value
                     ?? User.FindFirst("sub")?.Value
                     ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var userId))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null) return Unauthorized();

            // Invalider tout mécanisme de refresh côté serveur
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            // Bonus : invalider d’éventuelles sessions persistantes
            await _userManager.UpdateSecurityStampAsync(user);

            await _userManager.UpdateAsync(user);

            return Ok(new { message = "Déconnexion réussie" });
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            var res = await _authService.GetAllAsync();
            return Ok(res);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { message = "Validation échouée", details = errors });
            }

            _logger.LogInformation("API Register called");
            _logger.LogInformation("RegisterDto reçu : {@RegisterDto}", registerDto);

            if (registerDto == null)
            {
                return BadRequest(new { message = "Données manquantes" });
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

                return BadRequest(new { message = "Échec de l'inscription", details = errors });
            }

            return Ok(result.Data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            if (!result.Success)
                return Unauthorized(result.Errors);

            return Ok(result.Data);
        }

        [HttpGet("lookup")]
        public async Task<ActionResult<List<UserLookupDto>>> Lookup([FromQuery] string? q, [FromQuery] string? role, [FromQuery] int take = 20)
        {
            var res = await _authService.SearchAsync(q, role, take);
            if (!res.Success) return BadRequest(new { message = "Lookup échoué", errors = res.Errors });

            // Retour direct de la liste (plus simple côté front)
            return Ok(res.Data);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId =
                User.FindFirstValue("uid") ??
                User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Utilisateur non authentifié");

            var user = await _authService.GetByIdAsync(Guid.Parse(userId));
            if (user == null)
                return NotFound("Utilisateur introuvable");

            return Ok(user);
        }
    }
}
