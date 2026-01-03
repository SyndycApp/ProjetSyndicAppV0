using SyndicApp.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Common
{
    public interface INotificationService
    {
        Task NotifierAsync(
            Guid userId,
            string titre,
            string message,
            string type,
            Guid? cibleId = null,
            string? cibleType = null
        );

        Task<List<NotificationDto>> GetMesNotificationsAsync(Guid userId);
        Task MarquerCommeLueAsync(Guid notificationId, Guid userId);
    }

}
