using System;

namespace Saeed.Utilities.Constants
{
    public class Constants
    {
        public const string DefaultCorsPolicy = nameof(DefaultCorsPolicy);
        public const string AppName = "Axeto";
        public const string FaCulture = "fa-IR";
        public const string UsEnCulture = "en-US";
        public const string AndroidSmsAutoFillAxetoCode = "6eV4o4CfJH7";
        public const string AndroidSmsAutoFillAxitoCode = "5dcgkQUc7ZZ";

        public const string AcceptLanguageHeaderName = "Accept-Language";
        public const string AuthorizationHeaderName = "Authorization";
        public const string ContentTypeHeaderName = "ContentType";
        public const string RefererHeaderName = "Referer";
        public const string RabbitMQSettings = "RabbitMQSettings";
        public const string TempdataMessageTitle = "Message";
        public const string TempDataCookieName = "tempCooke";
        public const string ViewDataTitlePageName = "Title";
        public const string ServerBaseUrl = "https://axeto.net";
        public const string AppLogo = "https://sts.axeto.net/images/axeto.png";
        public const string SpCover = "https://sts.axeto.net/images/cover.jpg";
        public const string SpPanelUrl = "https://serviceprovider.axeto.net";
        public const string SpPanelRegister = "https://serviceprovider.axeto.net/register";

        public static string IdPayWebServiceToken { get { return "cc367444-4f22-4055-a409-788144496707"; } }

        public const string ProjectsCollection = "Projects";
        public const string ProjectDetailCollection = "ProjectDetails";
        public const string ServiceProvidersCollection = "ServiceProviders";
        public const string ServiceProviderOffersCollection = "ServiceProviderOffers";
        public const string AlbumsCollection = "Albums";
        public const string AlbumItemsCollection = "AlbumItems";
        public const string AlbumSettingsCollection = "AlbumSettings";
        public const string FilesCollection = "Files";
        public const string UserFilesCollection = "UserFiles";
        public const string ChatsCollection = "Chats";
        public const string ChatRoomsCollection = "ChatRooms";
        public const string NotificationsCollection = "Notifications";
        public const string JobsCollection = "Jobs";
        public const string CollectionsPrefix = "Axeto";
        public const string FilterServiceProviderForProject = "FilterServiceProviderForProject";
        public static string[] SupportedCultures
        {
            get => new[] { "fa-IR", "en-US" };
        }

        public static DateTime DateTimeUtcNow => DateTime.UtcNow;
    }
}
