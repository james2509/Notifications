using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notifications.Contract
{
    /// <summary>
    /// Represents a notification event to be stored and broadcast to
    /// subscribed listeners
    /// </summary>
    public class NotificationEvent
    {
        /// <summary>
        /// User Id that the event relates to. This is usually an email address.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Type of notification event to store and broadcast
        /// </summary>
        public EventType NotificationType { get; set; }

        /// <summary>
        /// Notification payload in JSon
        /// </summary>
        public string Data { get; set; }
    }
}
