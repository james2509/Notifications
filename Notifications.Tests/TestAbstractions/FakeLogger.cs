using System;
using System.Collections.Generic;
using System.Text;
using Notifications.Common.Interfaces;

namespace Notifications.Tests.TestAbstractions
{
    public class FakeLogger : INotificationsLogger
    {
        public void LogVerbose(string message)
        {
            
        }

        public void LogError(string message)
        {
            
        }

        public void LogException(Exception e)
        {
            
        }
    }
}
