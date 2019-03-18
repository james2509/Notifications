using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;

namespace Notifications.Hub
{
    public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly INotificationsService notificationsService;
        private readonly INotificationNotifyEvent notificationNotify;
        private readonly INotificationsLogger logger;

        public NotificationHub(INotificationsService notificationsService, INotificationNotifyEvent notificationNotify, INotificationsLogger logger)
        {
            this.notificationsService = notificationsService;
            this.notificationNotify = notificationNotify;
            this.logger = logger;

            
            notificationNotify.NotifyFunctionAsync = async model => await NotifyAsync(model);
        }

        /// <summary>
        /// A message is sent once a User starts a web socket session. This wil get all notifications for
        /// the User and send them on.
        /// </summary>
        public async Task initial(string userEmail)
        {
            notificationNotify.Client = Clients;
            try
            {
                if (notificationNotify.Connections.ContainsKey(userEmail))
                {
                    notificationNotify.Connections[userEmail] = Context.ConnectionId;
                    logger.LogVerbose($"Connection for {userEmail} changed to Id: {Context.ConnectionId.ToString()}");
                }
                else
                {
                    logger.LogVerbose($"Connection for {userEmail} established with Id: {Context.ConnectionId.ToString()}");
                    notificationNotify.Connections.Add(userEmail, Context.ConnectionId);
                }

                var notifications = notificationsService.GetUserNotifications(userEmail);

                if (notifications != null && notifications.Count > 0)
                {
                    foreach (var n in notifications)
                    {
                        var nJson = JsonConvert.SerializeObject(n);
                        await notificationNotify.Client.Caller.SendAsync("ReceiveNotifications", nJson);
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogException(e);
            }
        }

        /// <summary>
        /// Send a notification to a specific user if they have a web socket connection
        /// </summary>
        private async Task<bool> NotifyAsync(NotificationModel notification)
        {
            try
            {
                if (notificationNotify.Connections.ContainsValue(notification.User.Email))
                {
                    var connectionId = notificationNotify.Connections[notification.User.Email];
                    var nJson = JsonConvert.SerializeObject(notification);
                    await notificationNotify.Client.Client(connectionId).SendAsync("ReceiveNotifications", nJson);
                }
                else
                {
                    logger.LogVerbose($"No web socket sessions for User {notification.User.Email}");
                }
            }
            catch (Exception e)
            {
                logger.LogException(e);
                return false;
            }

            return true;
        }
    }
}
