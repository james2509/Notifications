using System;

namespace Notifications.Common.TestAbstractions
{
    public interface IClock
    {
        DateTime UtcNow();
    }

    public class Clock : IClock
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}