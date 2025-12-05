// SyndicApp.API/Controllers/PaiementsController.cs
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Finances;
using SyndicApp.Application.Interfaces.Finances;
using SyndicApp.Infrastructure.Services.Finances;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaiementsController : ControllerBase
    {
        private readonly IPaiementService _svc;
        public PaiementsController(IPaiementService svc) => _svc = svc;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PaiementDto>>> GetAll(CancellationToken ct)
            => Ok(await _svc.GetAllAsync(ct));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PaiementDto>> GetById(Guid id, CancellationToken ct)
        {
            var p = await _svc.GetByIdAsync(id, ct);
            return p is null ? NotFound() : Ok(p);
        }

        [HttpGet("by-appel/{appelId:guid}")]
        public async Task<IActionResult> GetByAppel(Guid appelId)
        {
            var paiements = await _svc.GetByAppelIdAsync(appelId);
            return Ok(paiements);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreatePaiementDto dto, CancellationToken ct)
        {
            try
            {
                var id = await _svc.CreateAsync(dto, ct);
                return CreatedAtAction(nameof(GetById), new { id }, new { id });
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}
