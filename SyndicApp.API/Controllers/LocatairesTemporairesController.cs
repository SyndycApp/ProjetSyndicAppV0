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
    public class LocatairesTemporairesController : ControllerBase
    {
        private readonly ILocataireTemporaireService _svc;
        public LocatairesTemporairesController(ILocataireTemporaireService svc) => _svc = svc;

        [HttpGet("by-lot/{lotId:guid}")]
        public async Task<ActionResult<IEnumerable<LocataireTemporaireDto>>> GetByLot(Guid lotId)
            => Ok(await _svc.GetByLotAsync(lotId));

        [HttpPost]
        public async Task<ActionResult> Create(CreateLocataireTemporaireDto dto)
        {
            var id = await _svc.CreateAsync(dto);
            return Ok(new { id });
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<LocataireTemporaireDto>>> GetAll()
        {
            var items = await _svc.GetAllAsync();
            return Ok(items);
        }


        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, UpdateLocataireTemporaireDto dto)
            => await _svc.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
            => await _svc.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
