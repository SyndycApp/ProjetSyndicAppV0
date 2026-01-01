using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.Interfaces.Assemblees;

namespace SyndicApp.API.Controllers
{
    [Authorize(Roles = "Syndic")]
    [ApiController]
    [Route("api/decisions")]
    public class DecisionsController : ControllerBase
    {
        private readonly IDecisionService _service;

        public DecisionsController(IDecisionService service)
        {
            _service = service;
        }

        // 🔹 Créer la décision d’une résolution
        [HttpPost("{resolutionId}")]
        public async Task<IActionResult> Creer(Guid resolutionId)
        {
            var decision = await _service.CreerDecisionAsync(resolutionId);
            return Ok(decision);
        }

        // 🔹 Lister les décisions d’une AG
        [HttpGet("assemblee/{assembleeId}")]
        public async Task<IActionResult> GetByAssemblee(Guid assembleeId)
        {
            return Ok(await _service.GetDecisionsByAssembleeAsync(assembleeId));
        }
    }
}
