using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.Services.Interfaces;
using Notifications.Services.Builders;

namespace Notifications.Services.Actions
{
    public class CreateNotificationSocketMessageSendAction : ICreateNotificationAction
    {
        private readonly INotificationsAccess notificationAccess;
        private readonly INotificationNotifyEvent notificationNotify;

        public CreateNotificationSocketMessageSendAction(INotificationsAccess notificationAccess, INotificationNotifyEvent notificationNotify)
        {
            this.notificationAccess = notificationAccess;
            this.notificationNotify = notificationNotify;
        }

        public int Order => 3;

        public async Task<NotificationCreateModel> Run(NotificationCreateModel notification)
        {
            var notificationModel = notificationAccess.GetAllNotifications().FirstOrDefault(n => n.Id == notification.Id);
   
            if (notificationModel != null)
            {
                var builders = NotificaionBuilder.LoadBuilders();
                var notificationTemplates = notificationAccess.GetAllNotificationTemplates().ToList();

                notificationModel = NotificaionBuilder.BuildNotification(notificationModel, builders, notificationTemplates);

                await notificationNotify.NotifyAsync(notificationModel);
            }
            
            return notification;
        }
    }
}
