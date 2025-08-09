using System.ComponentModel.DataAnnotations;

public class CreateResidenceDto
{
    [Required(ErrorMessage = "Le nom est obligatoire.")]
    [StringLength(100, ErrorMessage = "Le nom ne doit pas d�passer 100 caract�res.")]
    public string Nom { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'adresse est obligatoire.")]
    [StringLength(200, ErrorMessage = "L'adresse ne doit pas d�passer 200 caract�res.")]
    public string Adresse { get; set; } = string.Empty;

    [Required(ErrorMessage = "La ville est obligatoire.")]
    [StringLength(100, ErrorMessage = "La ville ne doit pas d�passer 100 caract�res.")]
    public string Ville { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le code postal est obligatoire.")]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "Le code postal doit contenir 5 chiffres.")]
    public string CodePostal { get; set; } = string.Empty;
}
