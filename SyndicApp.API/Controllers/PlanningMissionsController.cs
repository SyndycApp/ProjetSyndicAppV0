using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "Syndic")]
    [Route("api/planning-missions")]
    public class PlanningMissionsController : ControllerBase
    {
        private readonly IPlanningMissionService _service;

        public PlanningMissionsController(IPlanningMissionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePlanningMissionDto dto)
            => Ok(await _service.CreateAsync(dto));

        [HttpGet("employe/{id}")]
        public async Task<IActionResult> ByEmploye(Guid id)
            => Ok(await _service.GetByEmployeAsync(id));

        [HttpGet("residence/{id}")]
        public async Task<IActionResult> ByResidence(Guid id)
            => Ok(await _service.GetByResidenceAsync(id));
    }

}
