using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.Services.Interfaces;


namespace Notifications.Services.Actions
{
    public class CreateNotificationDatabaseCreateAction : ICreateNotificationAction
    {
        private readonly INotificationsAccess notificationAccess;
        private readonly INotificationNotifyEvent notificationNotify;

        public CreateNotificationDatabaseCreateAction(INotificationsAccess notificationAccess, INotificationNotifyEvent notificationNotify)
        {
            this.notificationAccess = notificationAccess;
            this.notificationNotify = notificationNotify;
        }

        public int Order => 2;

        public async Task<NotificationCreateModel> Run(NotificationCreateModel notification)
        {
            notification = await notificationAccess.CreateNotificationAsync(notification);
            return notification;
        }
    }
}
