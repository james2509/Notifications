using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notifications.Contract
{
    public class NotificationEvent
    {
        public string UserId { get; set; }

        public EventType NotificationType { get; set; }

        public string Data { get; set; }
    }
}
