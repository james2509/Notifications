using Notifications.Common.Interfaces;
using Notifications.Tests.TestAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Notifications.Common.Enumerations;
using Notifications.Common.Models;
using Notifications.Common.Models.NoticationDataTypes;
using Notifications.DataAccess.Access;
using Notifications.Services;
using Notifications.Services.Actions;
using Notifications.Services.Interfaces;
using Xunit;

namespace Notifications.Tests.Services.Actions
{
    public class CreateNotificationDatabaseCreateActionTest
    {
        private readonly NotificationsDbDataMother dbDataMother;
        private readonly INotificationsAccess notificationsAccess;
        private readonly ICreateNotificationAction action;
        private readonly FakeClock clock;

        public CreateNotificationDatabaseCreateActionTest()
        {
            clock = new FakeClock();
            dbDataMother = new NotificationsDbDataMother(clock);
            notificationsAccess = new NotificationsAccess(dbDataMother.Db, clock);
            action = new CreateNotificationDatabaseCreateAction(notificationsAccess);
        }

        [Fact]
        public void RunAddsRecordToTheDatabase()
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
            clock.AdvanceSeconds(66400);
            var result = action.Run(notification).Result;
            var storedNotification = dbDataMother.Db.Notifications.FirstOrDefault(n => n.DateCreated == clock.UtcNow());

            //Assert
            result.Should().NotBeNull();
            storedNotification.Should().NotBeNull();
        }
    }
}
