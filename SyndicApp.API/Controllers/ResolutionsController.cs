using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;

namespace SyndicApp.API.Controllers;

[ApiController]
[Route("api/assemblees/{assembleeId}/resolutions")]
[Authorize(Roles = "Syndic")]
public class ResolutionsController : ControllerBase
{
    private readonly IResolutionService _service;

    public ResolutionsController(IResolutionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Add(Guid assembleeId, CreateResolutionDto dto)
    {
        await _service.AddAsync(assembleeId, dto);
        return NoContent();
    }

    [HttpGet]
    [AllowAnonymous] // copropriétaires peuvent lire
    public async Task<IActionResult> Get(Guid assembleeId)
    {
        return Ok(await _service.GetByAssembleeAsync(assembleeId));
    }
}
