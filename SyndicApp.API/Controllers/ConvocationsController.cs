using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

    // 1️⃣ Créer une convocation
    [HttpPost]
    public async Task<IActionResult> Send(CreateConvocationDto dto)
    {
        await _service.SendAsync(dto);
        return NoContent();
    }

    // 2️⃣ Envoyer les emails
    [HttpPost("{convocationId:guid}/envoyer-emails")]
    public async Task<IActionResult> EnvoyerEmails(Guid convocationId)
    {
        await _service.EnvoyerEmailsAsync(convocationId);
        return NoContent();
    }

    // 3️⃣ Voir qui a lu / pas lu (syndic)
    [HttpGet("{convocationId:guid}/lecteurs")]
    public async Task<IActionResult> GetLecteurs(Guid convocationId)
    {
        var result = await _service.GetLecteursAsync(convocationId);
        return Ok(result);
    }

    // 4️⃣ Relancer les non-lecteurs
    [HttpPost("{convocationId:guid}/relancer")]
    public async Task<IActionResult> Relancer(Guid convocationId)
    {
        await _service.RelancerNonLecteursAsync(convocationId);
        return NoContent();
    }

    // 5️⃣ Marquer comme lue (copropriétaire)
    [Authorize] // copro autorisé
    [HttpPost("{convocationId:guid}/lue")]
    public async Task<IActionResult> MarquerCommeLue(Guid convocationId)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        await _service.MarquerCommeLueAsync(convocationId, userId);
        return NoContent();
    }
}
