using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.Interfaces.Personnel;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/personnel-dashboard")]
    [Authorize(Roles = "Syndic")]
    public class PersonnelDashboardController : ControllerBase
    {
        private readonly IPersonnelDashboardService _service;

        public PersonnelDashboardController(IPersonnelDashboardService service)
        {
            _service = service;
        }

        [HttpGet("{residenceId}")]
        public async Task<IActionResult> Get(Guid residenceId)
        {
            if (residenceId == Guid.Empty)
                return BadRequest("ResidenceId invalide.");

            var dashboard = await _service.GetAsync(residenceId);

            return Ok(dashboard);
        }
    }
}
