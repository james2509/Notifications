using System;
using System.Collections.Generic;
using System.Text;
using Notifications.Common.Interfaces;

namespace Notifications.Common.Loggers
{
    public class NotificationsConsoleLogger : INotificationsLogger
    {
        public void LogVerbose(string message)
        {
            Console.WriteLine($"Verbose: {message}");
        }

        public void LogError(string message)
        {
            Console.WriteLine($"Error: {message}");
        }

        public void LogException(Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
        }
    }
}
