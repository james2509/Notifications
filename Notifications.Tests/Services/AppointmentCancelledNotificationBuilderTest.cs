using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Notifications.Common.Enumerations;
using Notifications.Common.Models;
using Notifications.Common.Models.NoticationDataTypes;
using Notifications.DataAccess.Entities;
using Notifications.Services.Builders;
using Notifications.Tests.TestAbstractions;
using Xunit;
using FluentAssertions;

namespace Notifications.Tests.Services
{
    public class AppointmentCancelledNotificationBuilderTest
    {
        private NotificationTemplateModel template;
        private readonly FakeClock clock;
        private readonly AppointmentCancelledNotificationBuilder builder;

        public AppointmentCancelledNotificationBuilderTest()
        {
            clock = new FakeClock();
            builder = new AppointmentCancelledNotificationBuilder();
            template = new NotificationTemplateModel
            {
                Body =
                    "Hi {Firstname}, your appointment with {OrganisationName} at {AppointmentDateTime} has been - cancelled for the following reason: {Reason}.",
                Title = "Appointment Cancelled",
                Id = Guid.Parse("d9a2c131-202d-4bfb-b2ef-0b09a26d55d2"),
                NotificationType = NotificationTypeEnum.AppointmentCancelled
            };
        }

        [Fact]
        public void BuildGeneratesValidTitleAndBody()
        {
            //Arrange
            var notification = new NotificationModel
            {
                Id = Guid.NewGuid(),
                NotificationData = JObject.FromObject(new AppointmentCancelled
                {
                    AppointmentDateTime = new DateTime(2019, 03, 15, 15, 30, 0), OrganisationName = "Diddle Daddle Doctors",
                    Reason = "Understaffed"
                }),
                NotificationType = NotificationTypeEnum.AppointmentCancelled,
                User = new UserModel {FirstName = "Harlie", Id = new Guid()}
            };

            //Act
            notification = builder.Build(notification, template);

            //Assert
            notification.Body.Should().Be("Hi Harlie, your appointment with Diddle Daddle Doctors at Friday, 15 March 2019 15:30 has been - cancelled for the following reason: Understaffed.");
            notification.Title.Should().Be("Appointment Cancelled");
        }
      
    }
}
