using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;

namespace Notifications.Tests.TestAbstractions
{
    public class FakeNotifyEvent : INotificationNotifyEvent
    {
        public Func<NotificationModel, Task<bool>> NotifyFunctionAsync { get; set; }

        public IHubCallerClients Client { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Dictionary<string, string> Connections { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task NotifyAsync(NotificationModel notification)
        {
            return Task.CompletedTask;
        }
    }
}
