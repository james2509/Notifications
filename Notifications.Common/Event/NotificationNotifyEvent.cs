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
