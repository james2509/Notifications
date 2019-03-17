using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Notifications.Common.Models;

namespace Notifications.Common.Interfaces
{
    public interface INotificationsAccess
    {
        IEnumerable<NotificationModel> GetAllNotifications();

        IEnumerable<NotificationTemplateModel> GetAllNotificationTemplates();

        void CreateNotification(NotificationCreateModel notification);

        Task CreateNotificationAsync(NotificationCreateModel notification);
    }
}
