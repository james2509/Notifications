using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;

namespace Notifications.Common.Event
{
    /// <summary>
    /// Event class to handle raising notification events when notifications
    /// are created. This should be refactored so that Client and Connections
    /// are moved to a Context class to keep the Hub connection alive.
    /// </summary>
    public class NotificationNotifyEvent : INotificationNotifyEvent
    {
        public Func<NotificationModel, Task<bool>> NotifyFunctionAsync { get; set; }

        public IHubCallerClients Client { get; set; }

        public Dictionary<string, string> Connections { get; set; } = new Dictionary<string, string>();

        public async Task NotifyAsync(NotificationModel notification)
        {
            if (NotifyFunctionAsync != null)
            {
                await NotifyFunctionAsync(notification);
            }
        }
    }
}
