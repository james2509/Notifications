using System;
using System.Collections.Generic;
using System.Text;
using Notifications.Common.Interfaces;

namespace Notifications.Common.Models.NoticationDataTypes
{
    public class AppointmentCancelled
    {
        public DateTime AppointmentDateTime { get; set; }

        public string OrganisationName { get; set; }

        public string Reason { get; set; }
    }
}
