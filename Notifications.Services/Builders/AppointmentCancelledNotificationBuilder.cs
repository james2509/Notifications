using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using Notifications.Common.Models;
using Notifications.Common.Models.NoticationDataTypes;
using Notifications.Services.Interfaces;


namespace Notifications.Services.Builders
{
    public class AppointmentCancelledNotificationBuilder : INotificationBuilder
    {
        public string BuilderName => "AppointmentCancelledNotificationBuilder";

        public NotificationModel Build(NotificationModel notification, NotificationTemplateModel template)
        {
            var cancelType = notification.NotificationData.ToObject<AppointmentCancelled>();
            notification.Title = template.Title;
            notification.Body = template.Body.Replace("{OrganisationName}", cancelType.OrganisationName);
            notification.Body = notification.Body.Replace("{Reason}", cancelType.Reason);
            notification.Body = notification.Body.Replace("{AppointmentDateTime}", cancelType.AppointmentDateTime.ToString("dddd, dd MMMM yyyy HH:mm"));
            notification.Body = notification.Body.Replace("{Firstname}", notification.User.FirstName);

            return notification;
        }
    }
}
