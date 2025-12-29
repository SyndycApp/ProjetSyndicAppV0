using SyndicApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Personnel
{
    public record DeclareAbsenceDto
(
    AbsenceType Type,
    DateOnly Date,
    string? Motif
);
}
