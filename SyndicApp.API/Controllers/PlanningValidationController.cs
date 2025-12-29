using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;

namespace SyndicApp.API.Controllers
{
    [Authorize(Roles = "Syndic")]
    [ApiController]
    [Route("api/planning-validation")]
    public class PlanningValidationController : ControllerBase
    {
        private readonly IPlanningValidationService _service;

        public PlanningValidationController(IPlanningValidationService service)
        {
            _service = service;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> Validate(ValidateMissionDto dto)
        {
            await _service.ValidateAsync(dto);
            return Ok();
        }
    }

}
