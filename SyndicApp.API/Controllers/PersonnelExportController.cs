using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.Interfaces.Exports;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/personnel/exports")]
    [Authorize]
    public class PersonnelExportController : ControllerBase
    {
        private readonly IPresenceExportService _export;

        public PersonnelExportController(IPresenceExportService export)
        {
            _export = export;
        }


        [HttpGet("{employeId}/pdf")]
        public async Task<IActionResult> ExportPdf(
       Guid employeId,
       DateOnly from,
       DateOnly to)
        {
            var file = await _export.ExportPdfAsync(employeId, from, to);

            return File(
                file,
                "application/pdf",
                $"presences-{employeId}.pdf");
        }

        [HttpGet("{employeId}/excel")]
        public async Task<IActionResult> ExportExcel(
            Guid employeId,
            DateOnly from,
            DateOnly to)
        {
            var file = await _export.ExportExcelAsync(employeId, from, to);

            return File(
                file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"presences-{employeId}.xlsx");
        }
    }

}
