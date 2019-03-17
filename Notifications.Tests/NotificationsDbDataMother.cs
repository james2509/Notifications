using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Notifications.Common.Enumerations;
using Notifications.Common.TestAbstractions;
using Notifications.DataAccess;
using Notifications.DataAccess.Entities;
using Notifications.Tests.TestAbstractions;
using Newtonsoft;
using Newtonsoft.Json;
using Notifications.Common.Models.NoticationDataTypes;

namespace Notifications.Tests
{
    public class NotificationsDbDataMother
    {
        private readonly FakeClock clock;
        public NotificationsDbContext Db { get; }

        public NotificationsDbDataMother(FakeClock clock)
        {
            this.clock = clock;
            var options = new DbContextOptionsBuilder<NotificationsDbContext>()
                .UseSqlite("Data Source=:memory:;")
                .EnableSensitiveDataLogging();

            Db = new NotificationsDbContext(options.Options);
            Db.Database.OpenConnection();
            Db.Database.Migrate();
            SeedNotificationTestData();
        }

        ~NotificationsDbDataMother()
        {
            Db.Database.CloseConnection();
        }

        private void SeedNotificationTestData()
        {
            Db.Notifications.Add(GetNoticationEntity("james@test.com", 60, "Test reason"));
            Db.Notifications.Add(GetNoticationEntity("natalia@test.com", 10, "Test reason"));
            Db.SaveChanges();
        }

        private NotificationEntity GetNoticationEntity(string userEmail, int appointmentDateOffset, string reason)
        {
            var user = Db.NotificationUsers.First(u => String.Compare(userEmail, u.Email, true) == 0);
            var noticationType = Db.NotificationTypes.First(n => n.Id == 1);

            var appointmentCancelledType = new AppointmentCancelled
            {
                AppointmentDateTime = clock.UtcNow().AddDays(appointmentDateOffset),
                OrganisationName = "Doctors Inc",
                Reason = reason
            };

            var notification = new NotificationEntity
            {
                NotificationType = noticationType,
                User = user,
                DateCreated = clock.UtcNow(),
                NotificationData = JsonConvert.SerializeObject(appointmentCancelledType)
            };

            return notification;
        }
    }
}
