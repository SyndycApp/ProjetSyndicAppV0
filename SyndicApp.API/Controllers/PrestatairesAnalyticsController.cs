using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.Interfaces.Personnel;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/prestataires/analytics")]
    [Authorize(Roles = "Syndic")]
    public class PrestatairesAnalyticsController : ControllerBase
    {
        private readonly IPrestataireAnalyticsService _service;

        public PrestatairesAnalyticsController(
            IPrestataireAnalyticsService service)
        {
            _service = service;
        }

        [HttpGet("{prestataireId}")]
        public async Task<IActionResult> GetStats(
            Guid prestataireId,
            [FromQuery] DateOnly from,
            [FromQuery] DateOnly to)
        {
            var stats = await _service.GetStatsAsync(
                prestataireId, from, to);

            return Ok(stats);
        }
    }
}
