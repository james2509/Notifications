using System;
using System.Collections.Generic;
using System.Text;
using Notifications.Common.Models;

namespace Notifications.Services.Interfaces
{
    public interface INotificationBuilder
    {
        string BuilderName { get; }
        NotificationModel Build(NotificationModel notification, NotificationTemplateModel template);
    }
}
