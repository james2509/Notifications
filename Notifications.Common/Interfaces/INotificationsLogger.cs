using System;
using System.Collections.Generic;
using System.Text;

namespace Notifications.Common.Interfaces
{
    public interface INotificationsLogger
    {
        void LogVerbose(string message);

        void LogError(string message);

        void LogException(Exception e);
    }
}
