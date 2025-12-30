using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.Domain.Entities.Personnel;

namespace SyndicApp.Infrastructure.Services.Personnel
{
    public class AbsenceDocumentService : IAbsenceDocumentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        private static readonly string[] AllowedExtensions =
        {
            ".pdf", ".jpg", ".jpeg", ".png"
        };

        private const long MaxSizeBytes = 5 * 1024 * 1024; // 5 Mo

        public AbsenceDocumentService(
            ApplicationDbContext db,
            IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }


        public async Task<(byte[] Content, string FileName)> DownloadAsync(Guid justificationId)
        {
            var justification = await _db.AbsenceJustifications
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.Id == justificationId)
                ?? throw new InvalidOperationException("Justificatif introuvable.");

            if (string.IsNullOrWhiteSpace(justification.DocumentUrl))
                throw new InvalidOperationException("Aucun document associé.");

            var filePath = Path.Combine(
                _env.ContentRootPath,
                justification.DocumentUrl.TrimStart('/'));

            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException("Fichier introuvable.");

            var content = await System.IO.File.ReadAllBytesAsync(filePath);
            var fileName = Path.GetFileName(filePath);

            return (content, fileName);
        }


        public async Task UploadAsync(
            Guid justificationId,
            Guid uploadedByUserId,
            string fileName,
            byte[] content)
        {
            var justification = await _db.AbsenceJustifications
                .FirstOrDefaultAsync(j => j.Id == justificationId)
                ?? throw new InvalidOperationException("Justificatif introuvable.");

            if (justification.UserId != uploadedByUserId)
                throw new InvalidOperationException("Accès refusé.");

            // 🔐 Validation fichier
            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(ext))
                throw new InvalidOperationException("Type de fichier non autorisé.");

            if (content.Length > MaxSizeBytes)
                throw new InvalidOperationException("Fichier trop volumineux (max 5 Mo).");

            // 📁 Dossier
            var folder = Path.Combine(
                _env.ContentRootPath,
                "uploads",
                "absences",
                justification.UserId.ToString());

            Directory.CreateDirectory(folder);

            // 📄 Nom unique
            var storedFileName = $"{justification.Id}_{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(folder, storedFileName);

            // 💾 Écriture disque
            await File.WriteAllBytesAsync(fullPath, content);

            // 🧾 Lien DB
            justification.DocumentUrl =
                $"/uploads/absences/{justification.UserId}/{storedFileName}";

            await _db.SaveChangesAsync();
        }
    }
}
