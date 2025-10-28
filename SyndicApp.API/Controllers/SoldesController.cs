// SyndicApp.API/Controllers/SoldesController.cs
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.Interfaces.Finances;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    public class SoldesController : ControllerBase
    {
        private readonly ISoldeService _svc;
        public SoldesController(ISoldeService svc) => _svc = svc;

        [HttpGet("api/lots/{lotId:guid}/solde")]
        public async Task<ActionResult> GetSoldeLot(Guid lotId, CancellationToken ct)
        {
            var s = await _svc.GetSoldeLotAsync(lotId, ct);
            return Ok(s);
        }

        [HttpGet("api/residences/{residenceId:guid}/solde")]
        public async Task<ActionResult> GetSoldeResidence(Guid residenceId, CancellationToken ct)
        {
            var s = await _svc.GetSoldeResidenceAsync(residenceId, ct);
            return Ok(s);
        }
    }
}
