using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.Application.DTOs.Personnel;
using System.Threading.Tasks;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/personnel")]
    [Authorize(Roles = "Syndic")]
    public class PersonnelController : ControllerBase
    {
        private readonly IPersonnelService _service;
        private readonly IEmployeService _employeService;

        public PersonnelController(IPersonnelService service, IEmployeService employeService)
        {
            _service = service;
            _employeService = employeService;
        }

        // GET: api/personnel/interne
        [HttpGet("interne")]
        public async Task<IActionResult> GetPersonnelInterne()
        {
            var list = await _service.GetPersonnelInterneAsync();
            return Ok(list);
        }

        [Authorize(Roles = "Syndic")]
        [HttpPut("employes/{userId}")]
        public async Task<IActionResult> UpdateEmploye(Guid userId, EmployeUpdateDto dto)
        {
            await _employeService.UpdateEmployeAsync(userId, dto);
            return NoContent();
        }

        [HttpGet("{userId}/details")]
        public async Task<IActionResult> GetEmployeDetails(Guid userId)
        {
            return Ok(await _employeService.GetEmployeDetailsAsync(userId));
        }

    }
}
