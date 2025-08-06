namespace SyndicApp.Application.DTOs.Residences
{
    public class CreateResidenceDto
    {
        public string Nom { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
        public string Ville { get; set; } = string.Empty;
        public string CodePostal { get; set; } = string.Empty;
    }
}
