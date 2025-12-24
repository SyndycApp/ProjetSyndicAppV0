using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.Interfaces.Personnel;
using System.Threading.Tasks;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/personnel")]
    [Authorize(Roles = "Syndic")]
    public class PersonnelController : ControllerBase
    {
        private readonly IPersonnelService _service;

        public PersonnelController(IPersonnelService service)
        {
            _service = service;
        }

        // GET: api/personnel/interne
        [HttpGet("interne")]
        public async Task<IActionResult> GetPersonnelInterne()
        {
            var list = await _service.GetPersonnelInterneAsync();
            return Ok(list);
        }
    }
}
