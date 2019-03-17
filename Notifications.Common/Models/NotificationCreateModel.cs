using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json.Linq;
using Notifications.Common.Enumerations;

namespace Notifications.Common.Models
{
    public class NotificationCreateModel
    {
        public string UserEmail { get; set; }

        public NotificationTypeEnum NotificationType { get; set; }

        public dynamic EventData { get; set; }
    }
}
