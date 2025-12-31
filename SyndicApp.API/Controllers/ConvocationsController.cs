using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;

namespace SyndicApp.API.Controllers;

[ApiController]
[Route("api/convocations")]
[Authorize(Roles = "Syndic")]
public class ConvocationsController : ControllerBase
{
    private readonly IConvocationService _service;

    public ConvocationsController(IConvocationService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Send(CreateConvocationDto dto)
    {
        await _service.SendAsync(dto);
        return NoContent();
    }
}
