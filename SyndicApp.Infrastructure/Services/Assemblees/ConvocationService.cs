using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class ConvocationService : IConvocationService
    {
        private readonly ApplicationDbContext _db;

        public ConvocationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task SendAsync(CreateConvocationDto dto)
        {
            var convocation = new Convocation
            {
                AssembleeGeneraleId = dto.AssembleeGeneraleId,
                DateEnvoi = DateTime.UtcNow,
                Contenu = dto.Contenu
            };

            foreach (var userId in dto.DestinataireUserIds)
            {
                convocation.Destinataires.Add(new ConvocationDestinataire
                {
                    UserId = userId
                });
            }

            _db.Convocations.Add(convocation);
            await _db.SaveChangesAsync();
        }
    }

}
