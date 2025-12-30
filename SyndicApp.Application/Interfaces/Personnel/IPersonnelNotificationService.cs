using SyndicApp.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Personnel
{
    public interface IPersonnelNotificationService
    {
        Task MissionImminenteAsync(Guid userId, DateOnly date, TimeSpan heureDebut);
        Task RetardDetecteAsync(Guid userId);
        Task AbsenceDetecteeAsync(Guid userId, DateOnly date);
        Task MissionNonValideeAsync(Guid syndicUserId);
        Task<IReadOnlyList<Notification>> GetMyNotificationsAsync(Guid userId);
        Task MissionNonValideeAsync(Guid syndicUserId, Guid planningMissionId);
        Task MarkAsReadAsync(Guid notificationId);
    }

}
