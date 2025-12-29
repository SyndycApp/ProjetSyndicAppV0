using SyndicApp.Domain.Entities.Personnel;
using System.Text.Json.Serialization;

namespace SyndicApp.Application.DTOs.Personnel
{
    public class AffecterEmployeDto
    {
        [JsonPropertyName("employeId")]
        public Guid UserId { get; set; }
        public Guid ResidenceId { get; set; }
        public string Role { get; set; } = string.Empty;
    }

    public class AffectationEmployeDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public Guid ResidenceId { get; set; }
        public string RoleSurSite { get; set; } = string.Empty;

        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }

        public bool EstActive => DateFin == null;

        public AffectationEmployeDto() { }

        public AffectationEmployeDto(EmployeAffectationResidence entity)
        {
            Id = entity.Id;
            UserId = entity.UserId;              
            ResidenceId = entity.ResidenceId;
            RoleSurSite = entity.RoleSurSite;
            DateDebut = entity.DateDebut;
            DateFin = entity.DateFin;
        }
    }
}
