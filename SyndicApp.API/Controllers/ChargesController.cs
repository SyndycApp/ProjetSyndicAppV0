using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Finances;
using SyndicApp.Application.Interfaces.Finances;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChargesController : ControllerBase
    {
        private readonly IChargeService _svc;
        public ChargesController(IChargeService svc) => _svc = svc;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ChargeDto>>> GetAll(CancellationToken ct)
            => Ok(await _svc.GetAllAsync(ct));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ChargeDto>> GetById(Guid id, CancellationToken ct)
        {
            var c = await _svc.GetByIdAsync(id, ct);
            return c is null ? NotFound() : Ok(c);
        }

        // ?lotId=... pour rattacher à un lot
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateChargeDto dto, [FromQuery] Guid? lotId, CancellationToken ct)
        {
            try
            {
                var id = await _svc.CreateAsync(dto, lotId, ct);
                return CreatedAtAction(nameof(GetById), new { id }, new { id });
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateChargeDto dto, [FromQuery] Guid? lotId, CancellationToken ct)
        {
            try
            {
                var ok = await _svc.UpdateAsync(id, dto, lotId, ct);
                return ok ? NoContent() : NotFound();
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
            => await _svc.DeleteAsync(id, ct) ? NoContent() : NotFound();
    }
}
