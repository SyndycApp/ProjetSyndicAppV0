using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Audit
{
    public record AuditLogExportFilterDto(
    Guid? AssembleeId,
    DateTime? From,
    DateTime? To
);
    public record AuditLogDto(
    DateTime DateAction,
    string Action,
    string Cible,
    string Auteur
);
}
