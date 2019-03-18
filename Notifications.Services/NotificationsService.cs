using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.Services.Interfaces;
using Notifications.Services.Builders;

namespace Notifications.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly INotificationsAccess notificationsAccess;
        private readonly INotificationNotifyEvent notificationNotify;
        private readonly INotificationsLogger logger;

        public NotificationsService(INotificationsAccess notificationsAccess, INotificationNotifyEvent notificationNotify, INotificationsLogger logger)
        {
            this.notificationsAccess = notificationsAccess;
            this.notificationNotify = notificationNotify;
            this.logger = logger;
        }

        public IReadOnlyCollection<NotificationModel> GetAllNotifications()
        {
            var notifications = notificationsAccess.GetAllNotifications();
            return BuildNotifications(notifications);
        }

        public IReadOnlyCollection<NotificationModel> GetUserNotifications(string email)
        {
            var notifications = notificationsAccess.GetAllNotifications()
                .Where(u => String.Compare(email, u.User.Email, true) == 0);
            return BuildNotifications(notifications);
        }

        private IReadOnlyCollection<NotificationModel> BuildNotifications(IEnumerable<NotificationModel> notifications)
        {
            var builders = NotificaionBuilder.LoadBuilders();
            var notificationTemplates = notificationsAccess.GetAllNotificationTemplates().ToList();
            var readonlyNotifications = new List<NotificationModel>();

            try
            {
                foreach (var notification in notifications)
                {
                    readonlyNotifications.Add(NotificaionBuilder.BuildNotification(notification, builders, notificationTemplates));
                }
            }
            catch (Exception e)
            {
                logger.LogException(e);
            }

            return readonlyNotifications.AsReadOnly();
        }

        public async Task CreateNotificationAsync(NotificationCreateModel notification)
        {
            try
            {
                var actions = LoadCreateActions().OrderBy(o => o.Order);

                foreach (var action in actions)
                {
                    await action.Run(notification);
                }
            }
            catch (Exception e)
            {
                logger.LogException(e);
            }

        }

        private List<ICreateNotificationAction> LoadCreateActions()
        {
            var createActionInterface = typeof(ICreateNotificationAction);
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => createActionInterface.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => Activator.CreateInstance(x, notificationsAccess, notificationNotify) as ICreateNotificationAction)
                .OrderBy(o => o.Order)
                .ToList();
        }
    }
}
