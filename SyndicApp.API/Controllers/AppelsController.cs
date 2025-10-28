// SyndicApp.API/Controllers/AppelsController.cs
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
    public class AppelsController : ControllerBase
    {
        private readonly IAppelDeFondsService _svc;
        public AppelsController(IAppelDeFondsService svc) => _svc = svc;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppelDeFondsDto>>> GetAll(CancellationToken ct)
            => Ok(await _svc.GetAllAsync(ct));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AppelDeFondsDto>> GetById(Guid id, CancellationToken ct)
        {
            var a = await _svc.GetByIdAsync(id, ct);
            return a is null ? NotFound() : Ok(a);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateAppelDeFondsDto dto, CancellationToken ct)
        {
            try
            {
                var id = await _svc.CreateAsync(dto, ct);
                return CreatedAtAction(nameof(GetById), new { id }, new { id });
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAppelDeFondsDto dto, CancellationToken ct)
        {
            try
            {
                var ok = await _svc.UpdateAsync(id, dto, ct);
                return ok ? NoContent() : NotFound();
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("{id:guid}/cloturer")]
        public async Task<IActionResult> Cloturer(Guid id, CancellationToken ct)
            => await _svc.CloturerAsync(id, ct) ? NoContent() : NotFound();

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            try
            {
                return await _svc.DeleteAsync(id, ct) ? NoContent() : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
