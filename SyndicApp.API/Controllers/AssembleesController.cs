using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using System.Security.Claims;

namespace SyndicApp.API.Controllers;

[ApiController]
[Route("api/assemblees")]
[Authorize(Roles = "Syndic")]
public class AssembleesController : ControllerBase
{
    private readonly IAssembleeService _service;

    public AssembleesController(IAssembleeService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAssembleeDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue("uid")!);
        var id = await _service.CreateAsync(dto, userId);
        return Ok(id);
    }

    [HttpPost("{id}/publier")]
    public async Task<IActionResult> Publish(Guid id)
    {
        await _service.PublishAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/cloturer")]
    public async Task<IActionResult> Close(Guid id)
    {
        await _service.CloseAsync(id);
        return NoContent();
    }

    [HttpGet("upcoming/{residenceId}")]
    public async Task<IActionResult> Upcoming(Guid residenceId)
    {
        return Ok(await _service.GetUpcomingAsync(residenceId));
    }
}
