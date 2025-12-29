using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.Interfaces.Personnel;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/personnel-performance")]
    [Authorize(Roles = "Syndic")]
    public class PersonnelPerformanceController : ControllerBase
    {
        private readonly IPersonnelPerformanceService _service;

        public PersonnelPerformanceController(IPersonnelPerformanceService service)
        {
            _service = service;
        }

        [HttpGet("{employeId}")]
        public async Task<IActionResult> GetScore(Guid employeId)
        {
            if (employeId == Guid.Empty)
                return BadRequest("EmployeId invalide.");

            var result = await _service.GetScoreAsync(employeId);

            return Ok(result);
        }
    }
}
