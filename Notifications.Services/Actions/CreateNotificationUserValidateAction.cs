using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.Services.Interfaces;

namespace Notifications.Services.Actions
{
    public class CreateNotificationUserValidateAction : ICreateNotificationAction
    {
        private readonly INotificationsAccess notificationAccess;
        private readonly INotificationNotifyEvent notificationNotify;

        public CreateNotificationUserValidateAction(INotificationsAccess notificationAccess, INotificationNotifyEvent notificationNotify)
        {
            this.notificationAccess = notificationAccess;
            this.notificationNotify = notificationNotify;
        }

        public int Order => 1;

        Task<NotificationCreateModel> ICreateNotificationAction.Run(NotificationCreateModel notification)
        {
            return Task.FromResult(notification);
        }
    }
}
