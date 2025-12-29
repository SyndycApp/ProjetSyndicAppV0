using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;
using System.Security.Claims;

namespace SyndicApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/presence")]
public class PresenceController : ControllerBase
{
    private readonly IPresenceService _service;

    public PresenceController(IPresenceService service)
    {
        _service = service;
    }

    private Guid UserId =>
        Guid.Parse(User.FindFirstValue("uid")!);

    [HttpPost("start")]
    public async Task<IActionResult> Start(StartPresenceDto dto)
    {
        await _service.StartAsync(UserId, dto);
        return Ok();
    }

    [HttpPost("end")]
    public async Task<IActionResult> End()
    {
        await _service.EndAsync(UserId);
        return Ok();
    }

    [HttpGet("me")]
    public async Task<IActionResult> MyHistory()
    {
        return Ok(await _service.GetMyHistoryAsync(UserId));
    }
}
