using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Notifications.Common.Models;
using Notifications.Services.Interfaces;

namespace Notifications.Services.Builders
{
    public static class NotificaionBuilder
    {
        public static NotificationModel BuildNotification(NotificationModel notification, List<INotificationBuilder> builders,
            List<NotificationTemplateModel> notificationTemplates)
        {
            var template =
                notificationTemplates.FirstOrDefault(t => t.NotificationType == notification.NotificationType);

            if (template == null)
                throw new ArgumentNullException("Template", "Missing template");

            var builder = builders.First(b => b.BuilderName == template.BuilderClass);
            return builder.Build(notification, template);
        }

        public static List<INotificationBuilder> LoadBuilders()
        {
            var builderInterface = typeof(INotificationBuilder);
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => builderInterface.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => Activator.CreateInstance(x) as INotificationBuilder).ToList();
        }
    }
}
