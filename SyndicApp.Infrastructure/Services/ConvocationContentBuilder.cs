using Microsoft.AspNetCore.Hosting;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Entities.Residences;

public class ConvocationContentBuilder
{
    private readonly IWebHostEnvironment _env;

    public ConvocationContentBuilder(IWebHostEnvironment env)
    {
        _env = env;
    }
    public static string BuildHtml(
        IWebHostEnvironment env,
        AssembleeGenerale ag,
        Residence residence,
        IEnumerable<OrdreDuJourItem> ordreDuJour,
        string syndicNom)
    {
        var templatePath = Path.Combine(
            env.ContentRootPath,
            "Template",
            "ConvocationAg.html"
        );

        if (!File.Exists(templatePath))
            throw new FileNotFoundException(
                "Template convocation introuvable",
                templatePath
            );

        var html = File.ReadAllText(templatePath);

        var odjHtml = string.Join("",
            ordreDuJour
                .OrderBy(o => o.Ordre)
                .Select(o => $"<li>{o.Titre}</li>")
        );

        return html
            .Replace("{{TYPE_AG}}", ag.Type.ToString())
            .Replace("{{RESIDENCE}}", residence.Nom)
            .Replace("{{DATE}}", ag.DateDebut.ToString("dd/MM/yyyy"))
            .Replace("{{HEURE}}", ag.DateDebut.ToString("HH:mm"))
            .Replace("{{LIEU}}", residence.Adresse ?? "—")
            .Replace("{{ORDRE_DU_JOUR}}", odjHtml)
            .Replace("{{SYNDIC_NOM}}", syndicNom);
    }
}
