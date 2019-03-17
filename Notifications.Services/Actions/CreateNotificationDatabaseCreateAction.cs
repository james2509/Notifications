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

        public CreateNotificationDatabaseCreateAction(INotificationsAccess notificationAccess)
        {
            this.notificationAccess = notificationAccess;
        }

        public int Order => 2;

        public async Task<NotificationCreateModel> Run(NotificationCreateModel notification)
        {
            await notificationAccess.CreateNotificationAsync(notification);
            return notification;
        }
    }
}
