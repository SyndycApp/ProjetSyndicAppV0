using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using System.Security.Claims;

namespace SyndicApp.API.Controllers
{
    [Authorize(Roles = "Coproprietaire")]
    [ApiController]
    [Route("api/procurations")]
    public class ProcurationsController : ControllerBase
    {
        private readonly IProcurationService _service;

        public ProcurationsController(IProcurationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Donner(CreateProcurationDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("uid")!);
            await _service.DonnerProcurationAsync(userId, dto);
            return NoContent();
        }
    }

}
