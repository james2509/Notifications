using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.Services.Interfaces;

namespace Notifications.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly INotificationsAccess notificationsAccess;

        public NotificationsService(INotificationsAccess notificationsAccess)
        {
            this.notificationsAccess = notificationsAccess;
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
            var builders = LoadBuilders();
            var notificationTemplates = notificationsAccess.GetAllNotificationTemplates().ToList();
            var readonlyNotifications = new List<NotificationModel>();

            foreach (var notification in notifications)
            {
                var template =
                    notificationTemplates.FirstOrDefault(t => t.NotificationType == notification.NotificationType);

                if (template == null)
                    throw new ArgumentNullException("Template", "Missing template");

                var builder = builders.First(b => b.BuilderName == template.BuilderClass);
                readonlyNotifications.Add(builder.Build(notification, template));
            }

            return readonlyNotifications.AsReadOnly();
        }

        public async Task CreateNotificationAsync(NotificationCreateModel notification)
        {
            var actions = LoadCreateActions().OrderBy(o => o.Order);

            foreach (var action in actions)
            {
                await action.Run(notification);
            }
        }

        private List<INotificationBuilder> LoadBuilders()
        {
            var builderInterface = typeof(INotificationBuilder);
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => builderInterface.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => Activator.CreateInstance(x) as INotificationBuilder).ToList();
        }

        private List<ICreateNotificationAction> LoadCreateActions()
        {
            var createActionInterface = typeof(ICreateNotificationAction);
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => createActionInterface.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => Activator.CreateInstance(x, notificationsAccess) as ICreateNotificationAction)
                .OrderBy(o => o.Order)
                .ToList();
        }
    }
}
