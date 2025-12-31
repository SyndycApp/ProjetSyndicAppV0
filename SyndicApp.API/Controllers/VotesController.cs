using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using System.Security.Claims;

namespace SyndicApp.API.Controllers;

[ApiController]
[Route("api/votes")]
[Authorize(Roles = "Coproprietaire")]
public class VotesController : ControllerBase
{
    private readonly IVoteService _service;

    public VotesController(IVoteService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Vote(VoteDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue("uid")!);
        await _service.VoteAsync(userId, dto);
        return NoContent();
    }
}
