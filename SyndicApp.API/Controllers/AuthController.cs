using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Auth;
using SyndicApp.Application.Interfaces;
using SyndicApp.Infrastructure.Identity;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _logger = logger;
            _userManager = userManager;
        }

        // ---------------- LOGOUT ----------------
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var id = User.FindFirst("uid")?.Value
                     ?? User.FindFirst("sub")?.Value
                     ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(id))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return Unauthorized();

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userManager.UpdateSecurityStampAsync(user);
            await _userManager.UpdateAsync(user);

            return Ok(new { message = "Déconnexion réussie" });
        }

        // ---------------- GET ALL USERS ----------------
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            var res = await _authService.GetAllAsync();
            return Ok(res);
        }

        // ---------------- REGISTER USER ----------------
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
                return BadRequest(new { message = "Données manquantes" });

            var result = await _authService.RegisterAsync(registerDto);

            if (!result.Success)
            {
                List<string> errors;

                if (result.Errors == null)
                    errors = new List<string> { "Erreur inconnue" };
                else if (result.Errors is List<string> errorList)
                    errors = errorList;
                else
                    errors = new List<string> { result.Errors.ToString() ?? "Erreur inconnue" };

                return BadRequest(new { message = "Échec de l'inscription", details = errors });
            }

            return Ok(result.Data);
        }

        // ---------------- REGISTER PRESTATAIRE ----------------
        [HttpPost("register-from-prestataire-account")]
        public async Task<IActionResult> RegisterPrestataire([FromBody] RegisterPrestataireDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 1) Vérifier que le prestataire existe
            var prestataire = await _authService.GetPrestataireEntityAsync(dto.PrestataireId);
            if (prestataire == null)
                return NotFound(new { message = "Prestataire introuvable" });

            // 2) Créer l’utilisateur avec rôle = Prestataire
            var result = await _authService.RegisterAsync(new RegisterDto
            {
                FullName = prestataire.Nom,
                Email = dto.Email,
                Password = dto.Password,
                Adresse = prestataire.Adresse,
                DateNaissance = DateTime.UtcNow.AddYears(-30), // fake
                Role = "Prestataire"
            });

            if (!result.Success)
                return BadRequest(result.Errors);

            // 3) Associer le compte au prestataire
            await _authService.BindPrestataireToUserAsync(dto.PrestataireId, result.Data!.Id);

            return Ok(new
            {
                message = "Compte prestataire créé avec succès",
                result.Data!.Id,
                result.Data.Email,
                result.Data.FullName,
                result.Data.Roles
            });
        }

        // ---------------- LOGIN ----------------
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            if (!result.Success)
                return Unauthorized(result.Errors);

            return Ok(result.Data);
        }

        // ---------------- LOOKUP ----------------
        [HttpGet("lookup")]
        public async Task<ActionResult<List<UserLookupDto>>> Lookup(
            [FromQuery] string? q,
            [FromQuery] string? role,
            [FromQuery] int take = 20)
        {
            var res = await _authService.SearchAsync(q, role, take);
            if (!res.Success)
                return BadRequest(new { message = "Lookup échoué", errors = res.Errors });

            return Ok(res.Data);
        }

        // ---------------- ME ----------------
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> Me()
        {
            var userIdString =
                User.FindFirstValue("uid") ??
                User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized("Utilisateur non authentifié");

            if (!Guid.TryParse(userIdString, out var userGuid))
                return Unauthorized("Identifiant utilisateur invalide");

            var user = await _userManager.FindByIdAsync(userIdString);
            if (user is null)
                return NotFound("Utilisateur introuvable");

            var roles = await _userManager.GetRolesAsync(user);

            var dto = new UserDto
            {
                Id = userGuid,
                FullName = user.FullName,
                Email = user.Email,
                Roles = roles.ToList()
            };

            return Ok(dto);
        }
    }
}
