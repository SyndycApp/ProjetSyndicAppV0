using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Residences;
using SyndicApp.Application.Interfaces.Residences;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResidencesController : ControllerBase
    {
        private readonly IResidenceService _svc;

        public ResidencesController(IResidenceService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResidenceDto>>> GetAll()
        {
            var data = await _svc.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResidenceDto>> Get(Guid id)
        {
            var dto = await _svc.GetByIdAsync(id);
            if (dto is null) return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateResidenceDto dto)
        {
            var id = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [HttpGet("{id:guid}/lots")]
        public async Task<ActionResult<IReadOnlyList<LotDto>>> GetLots(Guid id, CancellationToken ct)
        {
            var lots = await _svc.GetLotsAsync(id, ct);
            return Ok(lots);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, UpdateResidenceDto dto)
        {
            var updated = await _svc.UpdateAsync(id, dto);
            return updated ? NoContent() : NotFound();
        }

        [HttpGet("{id:guid}/occupants")]
        public async Task<ActionResult<IReadOnlyList<ResidenceOccupantDto>>> GetOccupants(Guid id, CancellationToken ct)
        {
            var occupants = await _svc.GetOccupantsAsync(id, ct);
            return Ok(occupants);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var deleted = await _svc.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpGet("{id:guid}/details")]
        public async Task<ActionResult<ResidenceDetailsDto>> GetDetails(Guid id, CancellationToken ct)
        {
            var d = await _svc.GetResidenceDetailsAsync(id, ct);
            return d is null ? NotFound() : Ok(d);
        }
    }
}
