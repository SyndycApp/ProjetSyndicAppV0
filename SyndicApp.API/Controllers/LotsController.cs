using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Residences;
using SyndicApp.Application.Interfaces.Residences;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotsController : ControllerBase
    {
        private readonly ILotService _svc;
        public LotsController(ILotService svc) => _svc = svc;

        [HttpGet("by-residence/{residenceId:guid}")]
        public async Task<ActionResult<IEnumerable<LotDto>>> GetByResidence(Guid residenceId)
            => Ok(await _svc.GetByResidenceAsync(residenceId));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<LotDto>> Get(Guid id)
        {
            var dto = await _svc.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateLotDto dto)
        {
            var id = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<LotDto>>> GetAll()
        {
            var items = await _svc.GetAllAsync();
            return Ok(items);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, UpdateLotDto dto)
            => await _svc.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
            => await _svc.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
