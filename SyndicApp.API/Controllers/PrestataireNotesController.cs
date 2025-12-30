using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/prestataires/notes")]
    [Authorize(Roles = "Syndic")]
    public class PrestataireNotesController : ControllerBase
    {
        private readonly IPrestataireNoteService _service;

        public PrestataireNotesController(IPrestataireNoteService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Noter(
            PrestataireNoteCreateDto dto)
        {
            var syndicId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _service.AjouterOuMettreAJourAsync(syndicId, dto);
            return Ok();
        }

        [HttpGet("{prestataireId}")]
        public async Task<IActionResult> GetMoyenne(Guid prestataireId)
        {
            var result = await _service.GetMoyenneAsync(prestataireId);
            return Ok(result);
        }
    }
}
