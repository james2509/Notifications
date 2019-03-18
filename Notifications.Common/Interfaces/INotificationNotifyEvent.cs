using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Notifications.Common.Models;

namespace Notifications.Common.Interfaces
{
    public interface INotificationNotifyEvent
    {
        Func<NotificationModel, Task<bool>> NotifyFunctionAsync { get; set; }

        IHubCallerClients Client { get; set; }

        Dictionary<string, string> Connections { get; set; }

        Task NotifyAsync(NotificationModel notification);
    }
}
