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

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, UpdateBatimentDto dto)
            => await _svc.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
            => await _svc.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
