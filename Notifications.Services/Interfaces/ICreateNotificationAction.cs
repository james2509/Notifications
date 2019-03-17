using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Notifications.Common.Models;

namespace Notifications.Services.Interfaces
{
    public interface ICreateNotificationAction 
    {
        int Order { get; }

        Task<NotificationCreateModel> Run(NotificationCreateModel notification);
    }
}
