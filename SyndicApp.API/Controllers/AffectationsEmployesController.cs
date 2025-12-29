using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "Syndic")]
    [Route("api/employe-affectations")]
    public class AffectationsEmployesController : ControllerBase
    {
        private readonly IAffectationEmployeService _service;

        public AffectationsEmployesController(IAffectationEmployeService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Affecter(AffecterEmployeDto dto)
        {
            await _service.AffecterAsync(dto.UserId, dto.ResidenceId, dto.Role);
            return Ok();
        }

        [HttpPost("{id}/cloturer")]
        public async Task<IActionResult> Cloturer(Guid id)
        {
            await _service.CloturerAsync(id);
            return Ok();
        }
    }

}
