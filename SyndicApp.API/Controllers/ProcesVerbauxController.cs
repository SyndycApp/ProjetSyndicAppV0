using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.Interfaces.Assemblees;

namespace SyndicApp.API.Controllers;

[ApiController]
[Route("api/proces-verbaux")]
[Authorize(Roles = "Syndic")]
public class ProcesVerbauxController : ControllerBase
{
    private readonly IProcesVerbalService _service;

    public ProcesVerbauxController(IProcesVerbalService service)
    {
        _service = service;
    }

    [HttpPost("{assembleeId}/generer")]
    public async Task<IActionResult> Generate(Guid assembleeId)
    {
        await _service.GenerateAsync(assembleeId);
        return NoContent();
    }

    [HttpGet("{assembleeId}/telecharger")]
    public async Task<IActionResult> Telecharger(Guid assembleeId)
    {
        var (content, fileName) = await _service.GetPdfAsync(assembleeId);

        return File(
            content,
            "application/pdf",
            fileName
        );
    }
}
