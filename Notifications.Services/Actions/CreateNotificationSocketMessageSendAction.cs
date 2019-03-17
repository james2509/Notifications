using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.Services.Interfaces;

namespace Notifications.Services.Actions
{
    public class CreateNotificationSocketMessageSendAction : ICreateNotificationAction
    {
        private readonly INotificationsAccess notificationAccess;

        public CreateNotificationSocketMessageSendAction(INotificationsAccess notificationAccess)
        {
            this.notificationAccess = notificationAccess;
        }

        public int Order => 3;

        public Task<NotificationCreateModel> Run(NotificationCreateModel notification)
        {
            // TODO: implement send notification to user if the user has an active socket connection
            return Task.FromResult(notification);
        }
    }
}
