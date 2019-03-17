using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Notifications.Common.Models;

namespace Notifications.Common.Interfaces
{
    public interface INotificationsService
    {
        IReadOnlyCollection<NotificationModel> GetAllNotifications();

        IReadOnlyCollection<NotificationModel> GetUserNotifications(string email);

        Task CreateNotificationAsync(NotificationCreateModel notification);
    }
}
