using Microsoft.AspNetCore.Authorization;
using SyndicApp.Application.DTOs.Assemblees;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.Interfaces.Assemblees;
using System.Security.Claims;

namespace SyndicApp.API.Controllers
{
    [Authorize(Roles = "Coproprietaire")]
    [ApiController]
    [Route("api/presencesAss")]
    public class PresencesAsstroller : ControllerBase
    {
        private readonly IPresenceAssService _service;


        public PresencesAsstroller(IPresenceAssService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Presence(PresenceAssDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue("uid")!);
            await _service.EnregistrerPresenceAsync(userId, dto);
            return NoContent();
        }
    }
}
