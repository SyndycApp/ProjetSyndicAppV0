using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestatairesController : ControllerBase
    {
        private readonly IPrestataireService _service;

        public PrestatairesController(IPrestataireService service)
        {
            _service = service;
        }

        // GET: api/Prestataires?search=plombier
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search = null)
        {
            var list = await _service.GetAllAsync(search);
            return Ok(list);
        }

        // GET: api/Prestataires/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var p = await _service.GetByIdAsync(id);
            return p is null ? NotFound() : Ok(p);
        }

        // POST: api/Prestataires
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PrestataireCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/Prestataires/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PrestataireUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        // DELETE: api/Prestataires/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
