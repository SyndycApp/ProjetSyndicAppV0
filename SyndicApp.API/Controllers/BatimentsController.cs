using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Residences;
using SyndicApp.Application.Interfaces.Residences;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BatimentsController : ControllerBase
    {
        private readonly IBatimentService _svc;
        public BatimentsController(IBatimentService svc) => _svc = svc;

        [HttpGet("by-residence/{residenceId:guid}")]
        public async Task<ActionResult<IEnumerable<BatimentDto>>> GetByResidence(Guid residenceId)
            => Ok(await _svc.GetByResidenceAsync(residenceId));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BatimentDto>> Get(Guid id)
        {
            var dto = await _svc.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateBatimentDto dto)
        {
            var id = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BatimentDto>>> GetAll()
        {
            var items = await _svc.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("resolve-id")]
        public async Task<ActionResult<object>> ResolveId([FromQuery] string nom, CancellationToken ct)
        {
            var id = await _svc.ResolveIdByNameAsync(nom, ct);
            if (id is null) return NotFound();
            return Ok(new { id });
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, UpdateBatimentDto dto)
            => await _svc.UpdateAsync(id, dto) ? NoContent() : NotFound();
        [HttpGet("for-current-user")]
        public async Task<ActionResult<IEnumerable<BatimentDto>>> GetForCurrentUser(CancellationToken ct)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? User.FindFirstValue("sub");

            if (!Guid.TryParse(userIdStr, out var userId))
                return Forbid();

            var isSyndic = User.FindAll(ClaimTypes.Role)
                .Any(c => string.Equals(c.Value, "Syndic", StringComparison.OrdinalIgnoreCase));

            if (isSyndic)
                return Ok(await _svc.GetAllAsync(ct));

            var items = await _svc.GetForUserAsync(userId, ct);
            return Ok(items);
        }


        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
            => await _svc.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
