using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.API.Requests;
using System.Security.Claims;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/presence-validation")]
    [Authorize]
    public class PresenceValidationController : ControllerBase
    {
        private readonly IPresenceValidationService _service;
        private readonly IAbsenceDocumentService _absenceDocumentService;

        public PresenceValidationController(IPresenceValidationService service, IAbsenceDocumentService absenceDocumentService)
        {
            _service = service;
            _absenceDocumentService = absenceDocumentService;
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

        [HttpGet("absences/{justificationId}/document")]
        public async Task<IActionResult> DownloadJustificatif(Guid justificationId)
        {
            var (content, fileName) =
                await _absenceDocumentService.DownloadAsync(justificationId);

            return File(
                content,
                "application/octet-stream",
                fileName);
        }

        [HttpPost("absences/upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadJustificatif(
     [FromForm] UploadAbsenceJustificatifRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("Fichier manquant.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            using var ms = new MemoryStream();
            await request.File.CopyToAsync(ms);

            await _absenceDocumentService.UploadAsync(
                request.JustificationId,
                Guid.Parse(userId),
                request.File.FileName,
                ms.ToArray());

            return Ok(new { message = "Justificatif uploadé avec succès." });
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
