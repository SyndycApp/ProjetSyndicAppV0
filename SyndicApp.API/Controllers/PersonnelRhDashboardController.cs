using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.Interfaces.Personnel;

namespace SyndicApp.API.Controllers.Personnel
{
    [ApiController]
    [Route("api/personnel/rh-dashboard")]
    [Authorize]
    public class PersonnelRhDashboardController : ControllerBase
    {
        private readonly IPersonnelRhDashboardService _dashboardService;

        public PersonnelRhDashboardController(
            IPersonnelRhDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("{employeId:guid}")]
        public async Task<IActionResult> GetDetails(
            Guid employeId,
            [FromQuery] DateOnly from,
            [FromQuery] DateOnly to)
        {
            if (from > to)
                return BadRequest("La date de début doit être antérieure à la date de fin.");

            var result = await _dashboardService.GetDetailsAsync(
                employeId,
                from,
                to);

            return Ok(result);
        }
    }
}
