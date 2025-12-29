using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.Domain.Entities.Personnel;

namespace SyndicApp.Infrastructure.Services.Personnel
{
    public class EmployeDocumentService : IEmployeDocumentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public EmployeDocumentService(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task UploadAsync(Guid userId, UploadEmployeDocumentDto dto)
        {
            var employe = await _db.Employes.FindAsync(dto.EmployeId)
                ?? throw new InvalidOperationException("Employé introuvable.");

            // 📁 dossier
            var folder = Path.Combine(
                _env.ContentRootPath,
                "uploads",
                "employes",
                employe.Id.ToString()
            );

            Directory.CreateDirectory(folder);

            // 📄 nom unique
            var storedFileName = $"{Guid.NewGuid()}_{dto.FileName}";
            var path = Path.Combine(folder, storedFileName);

            // 💾 écriture disque
            await File.WriteAllBytesAsync(path, dto.Content);

            // 🗄️ DB
            _db.EmployeDocuments.Add(new EmployeDocument
            {
                EmployeId = employe.Id,
                Type = dto.Type,
                FileName = dto.FileName,          
                FilePath = path,                 
                FileSize = dto.Content.Length,
                UploadedByUserId = userId,
                CreatedAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<EmployeDocumentDto>> GetByEmployeAsync(Guid employeId)
        {
            return await _db.EmployeDocuments
                .Where(d => d.EmployeId == employeId)
                .OrderByDescending(d => d.CreatedAt)
                .Select(d => new EmployeDocumentDto(
                    d.Id,
                    d.Type,
                    d.FileName,
                    d.FileSize,
                    d.CreatedAt
                ))
                .ToListAsync();
        }

        public async Task<(byte[] Content, string FileName)> DownloadAsync(Guid documentId)
        {
            var doc = await _db.EmployeDocuments.FindAsync(documentId)
                ?? throw new InvalidOperationException("Document introuvable.");

            var content = await File.ReadAllBytesAsync(doc.FilePath);

            return (content, doc.FileName);
        }
    }
}
