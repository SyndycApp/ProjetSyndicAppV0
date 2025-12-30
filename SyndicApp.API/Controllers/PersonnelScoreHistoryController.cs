using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.Interfaces.Personnel;
using System;
using System.Threading.Tasks;

namespace SyndicApp.API.Controllers.Personnel
{
    [ApiController]
    [Route("api/personnel/scores")]
    [Authorize]
    public class PersonnelScoreHistoryController : ControllerBase
    {
        private readonly IPersonnelScoreHistoryService _scoreHistoryService;

        public PersonnelScoreHistoryController(
            IPersonnelScoreHistoryService scoreHistoryService)
        {
            _scoreHistoryService = scoreHistoryService;
        }


        [HttpPost("{employeId:guid}/generate")]
        public async Task<IActionResult> GenerateMonthlyScore(
            Guid employeId,
            [FromQuery] int annee,
            [FromQuery] int mois)
        {
            if (mois < 1 || mois > 12)
                return BadRequest("Le mois doit être compris entre 1 et 12.");

            await _scoreHistoryService.GenerateMonthlyScoreAsync(
                employeId,
                annee,
                mois);

            return NoContent();
        }

        [HttpGet("{employeId:guid}")]
        public async Task<IActionResult> GetHistory(Guid employeId)
        {
            var history = await _scoreHistoryService.GetHistoryAsync(employeId);
            return Ok(history);
        }
    }
}
