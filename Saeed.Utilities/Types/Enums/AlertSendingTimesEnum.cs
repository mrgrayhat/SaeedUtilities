using System.ComponentModel;

namespace Saeed.Utilities.Types.Enums
{
    /// <summary>
    /// زمان های ارسال هشدار
    /// </summary>
    public enum AlertSendingTimesEnum
    {
        [Description("هیچوقت")]
        NEVER = 1,
        [Description("هر ساعت")]
        EVERY_HOUR,
        [Description("هر دقیقه")]
        EVERY_MINUTE,
        [Description("روزانه")]
        EVERY_DAY,
        [Description("هفتگی")]
        EVERY_WEEK,
        [Description("ماهانه")]
        EVERY_MONTH,
        [Description("سالانه")]
        EVERY_YEAR,
    }
}