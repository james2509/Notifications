using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notifications.Common.Enumerations;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.Common.Models.NoticationDataTypes;
using Notifications.Contract;

namespace Notifications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService notificationsService;

        public NotificationsController(INotificationsService notificationsService)
        {
            this.notificationsService = notificationsService;
        }

        /// <summary>
        /// Api that returns all notifications stored or just notifcations
        /// for a specfic User
        /// </summary>
        [HttpGet]
        public IReadOnlyCollection<NotificationModel> Get(string userEmail)
        {
            if (!String.IsNullOrWhiteSpace(userEmail))
                return notificationsService.GetUserNotifications(userEmail);

            return notificationsService.GetAllNotifications();
        }

        /// <summary>
        /// Api to Create a notification event
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]NotificationEvent notification)
        {
            try
            {
                var t = GetType(notification.NotificationType);
                var dataObject = JObject.Parse(notification.Data).ToObject(t);
                var notificationModel = new NotificationCreateModel
                {
                    NotificationType = (NotificationTypeEnum)notification.NotificationType,
                    EventData = dataObject,
                    UserEmail = notification.UserId
                };
                await notificationsService.CreateNotificationAsync(notificationModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }

            return Ok();
        }

        private Type GetType(EventType eventType)
        {
            return
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.Name.Equals(eventType.ToString()));
        }
    }
}