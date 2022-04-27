using System;

namespace Saeed.Utilities.Constants
{
    public class Constants
    {
        public const string DefaultCorsPolicy = nameof(DefaultCorsPolicy);
        public const string FaCulture = "fa-IR";
        public const string UsEnCulture = "en-US";

        public const string AcceptLanguageHeaderName = "Accept-Language";
        public const string AuthorizationHeaderName = "Authorization";
        public const string ContentTypeHeaderName = "ContentType";
        public const string RefererHeaderName = "Referer";
        public const string RabbitMQSettings = "RabbitMQSettings";
        public const string TempdataMessageTitle = "Message";
        public const string TempDataCookieName = "tempCooke";
        public const string ViewDataTitlePageName = "Title";
        public static string[] SupportedCultures
        {
            get => new[] { "fa-IR", "en-US" };
        }
    }
}
