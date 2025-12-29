using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.API.Requests;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;
using System.Security.Claims;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/employes/documents")]
    [Authorize(Roles = "Syndic")]
    public class EmployeDocumentsController : ControllerBase
    {
        private readonly IEmployeDocumentService _service;

        public EmployeDocumentsController(IEmployeDocumentService service)
        {
            _service = service;
        }

        // ===============================
        // 📤 UPLOAD DOCUMENT RH
        // ===============================
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] UploadEmployeDocumentRequest request)
        {
            var userId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            // 🔁 IFormFile → byte[]
            using var ms = new MemoryStream();
            await request.File.CopyToAsync(ms);

            var dto = new UploadEmployeDocumentDto(
                request.EmployeId,
                request.Type,
                request.File.FileName,
                ms.ToArray()
            );

            await _service.UploadAsync(userId, dto);

            return Ok();
        }

        // ===============================
        // 📄 LISTE DES DOCUMENTS
        // ===============================
        [HttpGet("{employeId:guid}")]
        public async Task<ActionResult<IReadOnlyList<EmployeDocumentDto>>> GetByEmploye(Guid employeId)
        {
            var docs = await _service.GetByEmployeAsync(employeId);
            return Ok(docs);
        }

        // ===============================
        // ⬇️ TÉLÉCHARGEMENT
        // ===============================
        [HttpGet("download/{documentId:guid}")]
        public async Task<IActionResult> Download(Guid documentId)
        {
            var (content, fileName) = await _service.DownloadAsync(documentId);

            return File(
                content,
                "application/octet-stream",
                fileName
            );
        }
    }
}
