using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Interventions;
using SyndicApp.Application.Interfaces.Incidents;
using System;
using System.Threading.Tasks;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InterventionsController : ControllerBase
    {
        private readonly IInterventionService _interventionService;

        public InterventionsController(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        // POST: api/Interventions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InterventionCreateDto dto)
        {
            var result = await _interventionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result?.Id }, result);
        }

        // GET: api/Interventions/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var inter = await _interventionService.GetByIdAsync(id);
            return inter is null ? NotFound() : Ok(inter);
        }

        // GET: api/Interventions/by-residence/{residenceId}
        [HttpGet("by-residence/{residenceId:guid}")]
        public async Task<IActionResult> GetByResidence(Guid residenceId, int page = 1, int pageSize = 20)
        {
            var list = await _interventionService.GetByResidenceAsync(residenceId, page, pageSize);
            return Ok(list);
        }

        // PUT: api/Interventions/{id}/status
        [HttpPut("{id:guid}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] InterventionChangeStatusDto dto)
        {
            var result = await _interventionService.ChangeStatusAsync(id, dto);
            return result is null ? NotFound() : Ok(result);
        }

        // GET: api/Interventions
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var list = await _interventionService.GetAllAsync(page, pageSize);
            return Ok(list);
        }

        // DELETE: api/Interventions/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _interventionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
