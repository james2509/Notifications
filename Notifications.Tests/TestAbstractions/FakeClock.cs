using System;
using Notifications.Common.TestAbstractions;

namespace Notifications.Tests.TestAbstractions
{
    public class FakeClock : IClock
    {
        private DateTime _dateTime = new DateTime(2017, 9, 14, 21, 58, 23, 341, DateTimeKind.Utc);

        #region Implementation of IClock

        public DateTime UtcNow()
        {
            return _dateTime;
        }

        #endregion

        public void AdvanceSeconds(int seconds)
        {
            if (seconds == 0) return;
            _dateTime += TimeSpan.FromSeconds(seconds);
        }

        public void Set(DateTime time)
        {
            _dateTime = time;
        }
    }
}
