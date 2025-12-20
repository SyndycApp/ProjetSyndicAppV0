using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.AppelVocal;
using SyndicApp.Application.Interfaces.AppelVocal;
using SyndicApp.Domain.Entities.AppelVocal;
using SyndicApp.Domain.Enums;

namespace SyndicApp.Infrastructure.Services.AppelVocal
{
    public class CallService : ICallService
    {
        private readonly ApplicationDbContext _db;

        public CallService(ApplicationDbContext db)
        {
            _db = db;
            
        }

        public async Task<CallDto> StartCallAsync(Guid callerId, Guid receiverId)
        {
            var hasActiveCall = await _db.Calls.AnyAsync(c =>
                (c.CallerId == callerId || c.ReceiverId == callerId ||
                 c.CallerId == receiverId || c.ReceiverId == receiverId)
                && c.EndedAt == null
                && (c.Status == CallStatus.Ringing || c.Status == CallStatus.Accepted)
            );

            if (hasActiveCall)
                throw new InvalidOperationException("Utilisateur déjà en appel");

            var call = new Call
            {
                CallerId = callerId,
                ReceiverId = receiverId,
                StartedAt = DateTime.UtcNow,
                Status = CallStatus.Ringing
            };

            _db.Calls.Add(call);
            await _db.SaveChangesAsync();

            return Map(call);
        }


        public async Task AcceptCallAsync(Guid callId)
        {
            var call = await _db.Calls.FindAsync(callId);
            if (call == null) return;

            call.Status = CallStatus.Accepted;
            await _db.SaveChangesAsync();
        }

        public async Task EndCallAsync(Guid callId)
        {
            var call = await _db.Calls.FindAsync(callId);
            if (call == null || call.EndedAt != null)
                return;

            call.EndedAt = DateTime.UtcNow;
            call.Status = call.Status == CallStatus.Ringing
                ? CallStatus.Missed
                : CallStatus.Ended;

            await _db.SaveChangesAsync();
        }


        public async Task<IEnumerable<CallDto>> GetHistoryAsync(Guid userId)
        {
            return await _db.Calls
                .Where(c => c.CallerId == userId || c.ReceiverId == userId)
                .OrderByDescending(c => c.StartedAt)
                .Select(c => Map(c))
                .ToListAsync();
        }

        public async Task<IEnumerable<CallDto>> GetMissedCallsAsync(Guid userId)
        {
            return await _db.Calls
                .Where(c => c.ReceiverId == userId && c.Status == CallStatus.Missed)
                .Select(c => Map(c))
                .ToListAsync();
        }

        private static CallDto Map(Call c) => new()
        {
            Id = c.Id,
            CallerId = c.CallerId,
            ReceiverId = c.ReceiverId,
            StartedAt = c.StartedAt,
            EndedAt = c.EndedAt,
            DurationSeconds = c.DurationSeconds,
            Status = c.Status
        };
    }
}
