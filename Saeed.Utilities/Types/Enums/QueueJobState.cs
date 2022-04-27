using System.ComponentModel;

namespace Saeed.Utilities.Types.Enums
{
    /// <summary>
    /// Scheduled / Queue Job States.
    /// </summary>
    public enum QueueJobState
    {
        /// <summary>
        /// timeout or expiration reached
        /// </summary>
        [Description("منقضی شده")]
        Expired = -1,
        /// <summary>
        /// initial state. processing the job
        /// </summary>
        [Description("در حال پردازش")]
        Processing = 0,
        /// <summary>
        /// added to queue and waiting for start
        /// </summary>
        [Description("ورود به صف")]
        Enqueued = 1,
        /// <summary>
        /// removed from queue and finished
        /// </summary>
        [Description("خروج از صف")]
        Dequeued = 2,
        /// <summary>
        /// for a reason, job re-created again and waiting for start
        /// </summary>
        [Description("زمانبندی مجدد")]
        Requeued = 3,
        /// <summary>
        /// manually cancelled / removed
        /// </summary>
        [Description("لغو شده")]
        CANCELLED = 4,

    }
}
