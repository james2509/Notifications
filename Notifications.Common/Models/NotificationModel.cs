using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Notifications.Common.Enumerations;

namespace Notifications.Common.Models
{
    public class NotificationModel
    {
        public Guid Id { get; set; }

        public NotificationTypeEnum NotificationType { get; set; }

        public UserModel User { get; set; }

        public JObject NotificationData { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }
}
