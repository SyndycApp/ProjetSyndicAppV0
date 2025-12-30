namespace SyndicApp.Application.DTOs.Personnel
{
    public class PrestataireNoteCreateDto
    {
        public Guid PrestataireId { get; set; }
        public int Qualite { get; set; }
        public int Delai { get; set; }
        public int Communication { get; set; }
    }

    public class PrestataireNoteDto
    {
        public double Moyenne { get; set; }
        public int NbNotes { get; set; }
    }
}
