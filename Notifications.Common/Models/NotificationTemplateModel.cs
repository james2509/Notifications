using System;
using System.Collections.Generic;
using System.Text;
using Notifications.Common.Enumerations;

namespace Notifications.Common.Models
{
    public class NotificationTemplateModel
    {
        public Guid Id { get; set; }

        public NotificationTypeEnum NotificationType { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string BuilderClass { get; set; }
    }
}
