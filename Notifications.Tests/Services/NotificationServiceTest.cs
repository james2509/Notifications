using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Notifications.Common.Interfaces;
using Notifications.DataAccess.Access;
using Notifications.Services;
using Notifications.Tests.TestAbstractions;
using Xunit;
using FluentAssertions;
using Notifications.Common.Enumerations;
using Notifications.Common.Models;
using Notifications.Common.Models.NoticationDataTypes;

namespace Notifications.Tests.Services
{
    public class NotificationServiceTest
    {
        private readonly NotificationsDbDataMother dbDataMother;
        private readonly INotificationsAccess notificationsAccess;
        private readonly INotificationsService notificationsService;
        private readonly INotificationNotifyEvent notifyEvent;
        private readonly INotificationsLogger logger;
        private readonly FakeClock clock;

        public NotificationServiceTest()
        {
            clock = new FakeClock();
            notifyEvent = new FakeNotifyEvent();
            logger = new FakeLogger();
            dbDataMother = new NotificationsDbDataMother(clock);
            notificationsAccess = new NotificationsAccess(dbDataMother.Db, clock);
            notificationsService = new NotificationsService(notificationsAccess, notifyEvent, logger);
        }

        [Fact]
        public void GetAllNotificationsReturnsReadOnlyBuiltNotifications()
        {
            //Arrange
            
            //Act
            var results = notificationsService.GetAllNotifications();

            //Assert
            results.Should().NotBeNull();
            results.Count.Should().BeGreaterOrEqualTo(2);
        }

        [Fact]
        public void CreateNoticationsAsyncCreatesItemIfValid()
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
            clock.AdvanceSeconds(172800);
            notificationsService.CreateNotificationAsync(notification).Wait();
            var result = dbDataMother.Db.Notifications.First(n => n.DateCreated == clock.UtcNow());

            //Assert
            result.Should().NotBeNull();
        }
    }
}
