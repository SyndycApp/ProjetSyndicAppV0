using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Devis;
using SyndicApp.Application.Interfaces.Incidents;
using System;
using System.Threading.Tasks;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevisTravauxController : ControllerBase
    {
        private readonly IDevisTravauxService _devisService;

        public DevisTravauxController(IDevisTravauxService devisService)
        {
            _devisService = devisService;
        }

        // POST: api/DevisTravaux
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DevisCreateDto dto)
        {
            var result = await _devisService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result?.Id }, result);
        }

        // GET: api/DevisTravaux/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var devis = await _devisService.GetByIdAsync(id);
            return devis is null ? NotFound() : Ok(devis);
        }

        // GET: api/DevisTravaux/by-residence/{residenceId}
        [HttpGet("by-residence/{residenceId:guid}")]
        public async Task<IActionResult> GetByResidence(Guid residenceId, int page = 1, int pageSize = 20)
        {
            var devis = await _devisService.GetByResidenceAsync(residenceId, page, pageSize);
            return Ok(devis);
        }

        // PUT: api/DevisTravaux/{id}/decision
        [HttpPut("{id:guid}/decision")]
        public async Task<IActionResult> Decide(Guid id, [FromBody] DevisDecisionDto dto)
        {
            var result = await _devisService.DecideAsync(id, dto);
            return result is null ? NotFound() : Ok(result);
        }
        // GET: api/DevisTravaux
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var list = await _devisService.GetAllAsync(page, pageSize);
            return Ok(list);
        }


        // DELETE: api/DevisTravaux/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _devisService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("resolve-id")]
        public async Task<IActionResult> ResolveId([FromQuery] string titre)
        {
            if (string.IsNullOrWhiteSpace(titre))
                return BadRequest("Le paramètre 'titre' est obligatoire.");

            var devis = await _devisService.ResolveByTitleAsync(titre);
            return devis is null ? NotFound() : Ok(devis);
        }
    }
}
