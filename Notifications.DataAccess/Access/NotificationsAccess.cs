using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notifications.Common.Enumerations;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.Common.TestAbstractions;
using Notifications.DataAccess.Entities;

namespace Notifications.DataAccess.Access
{
    public class NotificationsAccess : INotificationsAccess
    {
        private readonly NotificationsDbContext dbContext;
        private readonly IClock clock;

        public NotificationsAccess(NotificationsDbContext dbContext, IClock clock)
        {
            this.clock = clock;
            this.dbContext = dbContext;
        }

        public IEnumerable<NotificationModel> GetAllNotifications()
        {
            return dbContext.Notifications.Select(n => new NotificationModel
            {
                Id = n.Id,
                NotificationData = JObject.Parse(n.NotificationData),
                NotificationType = (NotificationTypeEnum)n.NotificationType.Id,
                User = new UserModel { FirstName = n.User.FirstName, Id = n.User.Id, Email = n.User.Email}
            });
        }

        public IEnumerable<NotificationTemplateModel> GetAllNotificationTemplates()
        {
            return dbContext.NotificationTemplates.Select(t => new NotificationTemplateModel
            {
                Id = t.Id,
                NotificationType = (NotificationTypeEnum)t.NotificationTypeId,
                Body = t.Body,
                Title = t.Title,
                BuilderClass = t.BuilderClass
            });
        }

        public void CreateNotification(NotificationCreateModel notification)
        {
            CreateNotificationAsync(notification).Wait();
        }

        public async Task<NotificationCreateModel> CreateNotificationAsync(NotificationCreateModel notification)
        {
            var user = await dbContext.NotificationUsers.FirstOrDefaultAsync(u =>
                String.Compare(notification.UserEmail, u.Email, true) == 0);

            var notificationType =
                await dbContext.NotificationTypes.FirstAsync(n => n.Id == (Int16) notification.NotificationType);

            if (user == null)
                throw new ArgumentNullException("User", "User not found" );

            var notificationEntity = new NotificationEntity
            {
                NotificationType = notificationType,
                DateCreated = clock.UtcNow(),
                NotificationData = JsonConvert.SerializeObject(notification.EventData),
                User = user
            };

            await dbContext.Notifications.AddAsync(notificationEntity);
            await dbContext.SaveChangesAsync();
            notification.Id = notificationEntity.Id;

            return notification;
        }
    }
}
