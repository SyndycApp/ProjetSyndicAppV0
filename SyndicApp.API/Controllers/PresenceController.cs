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
    private readonly IPresenceMissionService _IpresenceServ;

    public PresenceController(IPresenceService service, IPresenceMissionService Ipresenceservice)
    {
        _service = service;
        _IpresenceServ = Ipresenceservice;
    }

    private Guid UserId =>
        Guid.Parse(User.FindFirstValue("uid")!);

    [HttpPost("start")]
    public async Task<IActionResult> Start(StartMissionPresenceDto dto)
    {
        var userId = Guid.Parse(User.FindFirst("uid")!.Value);
        await _IpresenceServ.StartAsync(userId, dto);
        return Ok();
    }

    [HttpPost("end")]
    public async Task<IActionResult> End(EndMissionPresenceDto dto)
    {
        var userId = Guid.Parse(User.FindFirst("uid")!.Value);
        await _IpresenceServ.EndAsync(userId, dto);
        return Ok();
    }

    [HttpGet("me")]
    public async Task<IActionResult> MyHistory()
    {
        return Ok(await _service.GetMyHistoryAsync(UserId));
    }
}
