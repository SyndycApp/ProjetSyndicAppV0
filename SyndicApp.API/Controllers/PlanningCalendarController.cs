using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.Interfaces.Personnel;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "Syndic")]
    [Route("api/planning/calendar")]
    public class PlanningCalendarController : ControllerBase
    {
        private readonly IPlanningCalendarService _service;

        public PlanningCalendarController(IPlanningCalendarService service)
        {
            _service = service;
        }

        // GET /planning/calendar?from=2025-01-01&to=2025-01-31
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] DateOnly from,
            [FromQuery] DateOnly to,
            [FromQuery] Guid? employeId,
            [FromQuery] Guid? residenceId)
        {
            return Ok(await _service.GetAsync(from, to, employeId, residenceId));
        }
    }

}
