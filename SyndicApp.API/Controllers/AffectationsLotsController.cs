using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Residences;
using SyndicApp.Application.Interfaces.Residences;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;               // ✅ pour CancellationToken
using System.Threading.Tasks;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AffectationsLotsController : ControllerBase
    {
        private readonly IAffectationLotService _svc;
        public AffectationsLotsController(IAffectationLotService svc) => _svc = svc;

        // ----------- EXISTANTS -----------
        [HttpGet("by-lot/{lotId:guid}")]
        public async Task<ActionResult<IEnumerable<AffectationLotDto>>> GetByLot(Guid lotId)
            => Ok(await _svc.GetByLotAsync(lotId));

        [HttpGet("by-user/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<AffectationLotDto>>> GetByUser(Guid userId)
            => Ok(await _svc.GetByUserAsync(userId));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AffectationLotDto>> GetById(Guid id, CancellationToken ct)
        {
            var dto = await _svc.GetByIdAsync(id, ct);
            if (dto is null) return NotFound();
            return Ok(dto);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AffectationLotDto>>> GetAll()
        {
            var items = await _svc.GetAllAsync();
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateAffectationLotDto dto)
        {
            try
            {
                var id = await _svc.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id }, new { id });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, UpdateAffectationLotDto dto, CancellationToken ct)
        {
            var updated = await _svc.UpdateAsync(id, dto, ct);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpPut("{id:guid}/cloturer")]
        public async Task<ActionResult> Cloturer(Guid id, ClotureAffectationLotDto dto)
            => await _svc.CloturerAsync(id, dto.DateFin) ? NoContent() : NotFound();

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
            => await _svc.DeleteAsync(id) ? NoContent() : NotFound();

        [HttpGet("lot/{lotId:guid}/historique")]
        public async Task<ActionResult<IEnumerable<AffectationHistoriqueDto>>> GetHistoriqueByLot(Guid lotId, CancellationToken ct)
        {
            var historiques = await _svc.GetHistoriqueByLotAsync(lotId, ct);
            return Ok(historiques);
        }

        [HttpGet("lot/{lotId:guid}/occupant-actuel")]
        public async Task<ActionResult<AffectationHistoriqueDto>> GetOccupantActuel(Guid lotId, CancellationToken ct)
        {
            var occupant = await _svc.GetOccupantActuelAsync(lotId, ct);
            if (occupant is null) return NotFound();
            return Ok(occupant);
        }

        // ----------- AJOUTS (Gestion locataires) -----------

        // Assigner un locataire (EstProprietaire = false)
        [HttpPost("assigner-locataire")]
        public async Task<ActionResult<Guid>> AssignerLocataire([FromBody] CreateAffectationLotDto dto, CancellationToken ct)
        {
            dto.EstProprietaire = false; // locataire
            var id = await _svc.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetHistoriqueByLot), new { lotId = dto.LotId }, id);
        }

        // Changer le statut propriétaire/locataire
        [HttpPut("{id:guid}/changer-statut")]
        public async Task<IActionResult> ChangerStatut(Guid id, [FromBody] UpdateAffectationLotDto dto, CancellationToken ct)
        {
            if (dto.EstProprietaire is null)
                return BadRequest("EstProprietaire obligatoire pour ce endpoint.");

            var ok = await _svc.UpdateAsync(id, dto, ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpGet("for-current-user")]
        public async Task<ActionResult<IReadOnlyList<AffectationLotDto>>> GetForCurrentUser(CancellationToken ct)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? User.FindFirstValue("sub");

            if (!Guid.TryParse(userIdStr, out var userId))
                return Forbid();

            var isSyndic = User.FindAll(ClaimTypes.Role)
                .Any(c => string.Equals(c.Value, "Syndic", StringComparison.OrdinalIgnoreCase));

            if (isSyndic)
                return Ok(await _svc.GetAllAsync(ct));

            var items = await _svc.GetByUserAsync(userId, ct);
            return Ok(items);
        }


        // Occupant actuel d’un lot (route alternative côté /api/lots/…)
        [HttpGet("~/api/lots/{lotId:guid}/occupant-actuel")]
        public async Task<ActionResult<AffectationHistoriqueDto?>> GetOccupantActuelAlt(Guid lotId, CancellationToken ct)
        {
            var occ = await _svc.GetOccupantActuelAsync(lotId, ct);
            return occ is null ? NotFound() : Ok(occ);
        }
    }
}
