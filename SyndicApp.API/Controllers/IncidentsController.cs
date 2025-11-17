using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Incidents;
using SyndicApp.Application.Interfaces.Incidents;
using System;
using System.Threading.Tasks;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentsController : ControllerBase
    {
        private readonly IIncidentService _incidentService;

        public IncidentsController(IIncidentService incidentService)
        {
            _incidentService = incidentService;
        }

        // POST: api/Incidents
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IncidentCreateDto dto)
        {
            var result = await _incidentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result?.Id }, result);
        }

        // GET: api/Incidents/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var incident = await _incidentService.GetByIdAsync(id);
            return incident is null ? NotFound() : Ok(incident);
        }

        // GET: api/Incidents/by-residence/{residenceId}
        [HttpGet("by-residence/{residenceId:guid}")]
        public async Task<IActionResult> GetByResidence(Guid residenceId, int page = 1, int pageSize = 20)
        {
            var incidents = await _incidentService.GetByResidenceAsync(residenceId, page, pageSize);
            return Ok(incidents);
        }

        // PUT: api/Incidents/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] IncidentUpdateDto dto)
        {
            var updated = await _incidentService.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        // PUT: api/Incidents/{id}/status
        [HttpPut("{id:guid}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] IncidentChangeStatusDto dto)
        {
            var result = await _incidentService.ChangeStatusAsync(id, dto);
            return result is null ? NotFound() : Ok(result);
        }

        // GET: api/Incidents
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var list = await _incidentService.GetAllAsync(page, pageSize);
            return Ok(list);
        }


        [HttpGet("resolve-id")]
        public async Task<IActionResult> ResolveId([FromQuery] string titre)
        {
            if (string.IsNullOrWhiteSpace(titre))
                return BadRequest("Le paramètre 'titre' est obligatoire.");

            var incident = await _incidentService.ResolveByTitleAsync(titre);
            return incident is null ? NotFound() : Ok(incident);
        }

        // DELETE: api/Incidents/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _incidentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
