using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notifications.Common.Enumerations;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.Common.Models.NoticationDataTypes;
using Notifications.Common.TestAbstractions;
using Notifications.DataAccess.Access;
using Notifications.DataAccess.Entities;
using Notifications.Tests.TestAbstractions;

namespace Notifications.Tests.DataAccess
{
    public class NotificationsAccessTest
    {
        private readonly NotificationsDbDataMother dbDataMother;
        private readonly INotificationsAccess notificationsAccess;
        private readonly FakeClock clock;

        public NotificationsAccessTest()
        {
            clock = new FakeClock();
            dbDataMother = new NotificationsDbDataMother(clock);
            notificationsAccess = new NotificationsAccess(dbDataMother.Db, clock);
        }

        [Fact]
        public void GetAllNotificationsReturnsAllNotificationsStored()
        {
            //Arrange
            var expectedCount = dbDataMother.Db.Notifications.Count();

            //Act
            var results = notificationsAccess.GetAllNotifications().ToList();

            //Assert
            results.Should().NotBeNull();
            results.Should().NotContainNulls();
            results.Count.Should().Be(expectedCount);
        }

        [Fact]
        public void GetAllNotificationsReturnsWellFormedDataTypeObject()
        {
            //Arrange

            //Act   
            var result = notificationsAccess.GetAllNotifications().First();
            var appointmentCancelledType = result.NotificationData.ToObject<AppointmentCancelled>();

            //Assert
            appointmentCancelledType.Should().NotBeNull();
            appointmentCancelledType.OrganisationName.Should().Be("Doctors Inc");
        }

        [Fact]
        public void GetAllNotificationTemplatesReturnsAllStoredNotificationTemplates()
        {
            //Arrange
            var expectedCount = dbDataMother.Db.NotificationTemplates.Count();

            //Act
            var results = notificationsAccess.GetAllNotificationTemplates().ToList();

            //Assert
            results.Should().NotBeNull();
            results.Should().NotContainNulls();
            results.Count.Should().Be(expectedCount);
        }

        [Fact]
        public void CreateNotificationUserNotFoundThrowsException()
        {
            //Arrange
            var notification = new NotificationCreateModel
            {
                EventData = new AppointmentCancelled
                {
                    AppointmentDateTime = clock.UtcNow(),
                    OrganisationName = "Village square surgery",
                    Reason = "Add test reason"
                },
                NotificationType = NotificationTypeEnum.AppointmentCancelled,
                UserEmail = "bob@test.com"
            };

            //Act
            Action action = () => notificationsAccess.CreateNotificationAsync(notification).Wait();
            
            //Assert
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("*User not found*");
        }

        [Fact]
        public void CreateNotificationStoresNewNotification()
        {
            //Arrange
            var appointmentDate = clock.UtcNow().AddDays(45);
            var notification = new NotificationCreateModel
            {
                EventData = new AppointmentCancelled
                {
                    AppointmentDateTime = appointmentDate,
                    OrganisationName = "Village square surgery",
                    Reason = "Add test reason"
                },
                NotificationType = NotificationTypeEnum.AppointmentCancelled,
                UserEmail = "natalia@test.com"
            };

            //Act
            clock.AdvanceSeconds(86400);
            notificationsAccess.CreateNotificationAsync(notification).Wait();
            var result = dbDataMother.Db.Notifications.FirstOrDefault(n => n.DateCreated == clock.UtcNow());
            var cancelType = JObject.Parse(result.NotificationData).ToObject<AppointmentCancelled>();

            //Assert
            result.Should().NotBeNull();
            cancelType.Should().NotBeNull();
            cancelType.AppointmentDateTime.Should().Be(appointmentDate);
        }
    }
}
