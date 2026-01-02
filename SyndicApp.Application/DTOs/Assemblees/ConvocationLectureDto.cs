using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Assemblees
{
    public record ConvocationLectureDto(
    Guid UserId,
    string Nom,
    bool Lu,
    DateTime? LuLe
);
}
