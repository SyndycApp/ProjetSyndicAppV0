using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;
using System.Security.Claims;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/presence-validation")]
    [Authorize]
    public class PresenceValidationController : ControllerBase
    {
        private readonly IPresenceValidationService _service;

        public PresenceValidationController(IPresenceValidationService service)
        {
            _service = service;
        }


        [HttpGet("non-validees")]
        public async Task<ActionResult<IReadOnlyList<AbsenceJustificationDto>>> GetNonValidees()
       => Ok(await _service.GetNonValideesAsync());

        // 🟢 VALIDÉES
        [HttpGet("validees")]
        public async Task<ActionResult<IReadOnlyList<AbsenceJustificationDto>>> GetValidees()
            => Ok(await _service.GetValideesAsync());

        // =========================================================
        // 👤 EMPLOYÉ – Déclarer une absence / retard / congé
        // POST /api/presence-validation/declarer
        // =========================================================
        [HttpPost("declarer")]
        public async Task<IActionResult> Declarer([FromBody] DeclareAbsenceDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            await _service.DeclarerAsync(Guid.Parse(userId), dto);

            return Ok();
        }

        // =========================================================
        // 👔 SYNDIC / RH – Valider un justificatif
        // POST /api/presence-validation/valider/{id}
        // =========================================================
        [HttpPost("valider/{id:guid}")]
        [Authorize(Roles = "Syndic")]
        public async Task<IActionResult> Valider(Guid id)
        {
            await _service.ValiderAsync(id);
            return Ok();
        }
    }
}
