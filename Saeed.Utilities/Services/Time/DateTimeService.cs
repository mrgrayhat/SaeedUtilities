using System;

namespace Saeed.Utilities.Services.Time
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
        DateTime Now { get; }
        TimeSpan Offset { get; }
        DateTimeOffset NowUtcOffset { get; }
        DateTimeOffset NowOffset { get; }
    }

    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
        public DateTime Now => DateTime.Now;
        public TimeSpan Offset => DateTimeOffset.Now.Offset;
        public DateTimeOffset NowUtcOffset => DateTimeOffset.UtcNow;
        public DateTimeOffset NowOffset => DateTimeOffset.Now;

    }
}
