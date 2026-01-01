using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.Interfaces.Assemblees;
using System.Security.Claims;

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

    [HttpPost("{assembleeId}")]
    public async Task<IActionResult> Generate(Guid assembleeId)
    {
        var syndicId = Guid.Parse(User.FindFirstValue("uid")!);
        await _service.GenerateAsync(assembleeId, syndicId);
        return NoContent();
    }

    [HttpGet("{assembleeId}/pdf")]
    public async Task<IActionResult> GetPdf(Guid assembleeId)
    {
        var (content, fileName) = await _service.GetPdfAsync(assembleeId);
        return File(content, "application/pdf", fileName);
    }

    
}
