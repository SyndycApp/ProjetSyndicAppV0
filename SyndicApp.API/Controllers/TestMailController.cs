using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.Interfaces.Common;

[ApiController]
[Route("api/Atest-mail")]
public class TestMailController : ControllerBase
{
    private readonly IMailService _mail;

    public TestMailController(IMailService mail)
    {
        _mail = mail;
    }

    [HttpPost]
    public async Task<IActionResult> Send()
    {
        await _mail.EnvoyerAsync(
            "elalaouiomar1998@gmail.com",
            "TEST SMTP SyndicApp",
            "<h2>Test email OK</h2><p>Si tu vois ce mail, SMTP fonctionne.</p>"
        );

        return Ok("Mail envoyé");
    }
}
