
using Saeed.Utilities.Types.ValueObjects;

namespace Saeed.Utilities.Constants
{
    /// <summary>
    /// Cache Ket Prefix builder pool / factory
    /// </summary>
    public static class CacheKeyBuilder
    {
        public static KeyPrefixObject Create(in string prefix)
        {
            return new KeyPrefixObject(prefix);
        }
        public static KeyPrefixObject Create(in string prefix, in string[] args)
        {
            return new KeyPrefixObject(prefix, args);
        }

        public static void Destroy(KeyPrefixObject keyPrefixObject)
        {
            //Pool.Return(keyPrefixObject);

            Garbage();
        }

        public static void Garbage()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect(2, GCCollectionMode.Optimized, false, true);
        }
    }
    /// <summary>
    /// caching system base key prefixs which is used for complex key building and identity of cache items in cache store
    /// </summary>
    public sealed class CacheKeyPrefixes
    {
        public const string App = "app";
        public const string AppSettings = "app:settings";
        public const string Online = "online";
        public const string WebRootPath = "webrootpath";
        public const string BaseUrl = "baseUrl";
        public const string Count = "count";
        public const string Version = "version";
        public const string Status = "status";
        public const string State = "state";
        public const string Type = "type";
        public const string Availability = "availability";
        public const string Requests = "requests";
        public const string Unreads = "unreads";
        public const string Active = "active";
        public const string Access = "access";
        public const string Confirm = "confirm";

        public const string ActionPoints = "actionpoints";
        public const string InOuts = "inouts";

        public const string Cities = "cities";
        public const string Provinces = "provinces";
        public const string Countries = "countries";
        public const string Addresses = "addresses";
        public const string UserAddresses = "users:customers:profile:addresses";

        public const string MailServers = "mailservers";
        public const string Clients = "clients";
        public const string Emails = "emails";
        public const string Email = "email";
        public const string SmsProviders = "smss:providers";
        public const string Smss = "smss";
        public const string Chats = "chats";
        public const string ChatRooms = "chats:rooms";
        public const string Country = "app:country";
        public const string Categories = "app:homedata:categories";
        public const string States = "appp:states";
        public const string HomeData = "app:homedata";
        public const string Banners = "app:homedata:banners";
        public const string Sliders = "app:homedata:sliders";
        public const string DiscountCodes = "discounts:codes";
        public const string Transactions = "transactions";
        public const string Payments = "payments";

        public const string Projects = "projects";
        public const string ProjectChats = "projects:chats";
        public const string ProjectInfo = "projects:info";
        public const string ProjectPhotos = "projects:photos";
        public const string Offers = "projects:offers";
        public const string ProjectOffers = "projects:offers";
        public const string ProjectComments = "projects:comments";

        public const string Users = "users";
        public const string Username = "username";
        public const string UserId = "userId";
        public const string DeviceId = "deviceId";
        public const string Code = "code";

        public const string UserDevices = "users:logins:devices";
        public const string UserProfile = "users:profile";
        public const string UserChats = "users:chats";
        public const string UserLogins = "users:logins";
        public const string UserFcmTokens = "users:logins:tokens";

        public const string ServiceProviders = "users:serviceProviders";
        public const string ServiceProviderInfo = "users:serviceProviders:info";
        public const string ServiceProviderProfile = "users:serviceProviders:profile";
        public const string ServiceProviderApplicationProfile = "users:serviceProviders:application:profile";
        public const string ServiceProviderPortfolio = "users:serviceProviders:portfolios";
        public const string ServiceProviderPortfolioItem = "users:serviceProviders:portfolios:item";
        public const string ServiceProviderProjects = "users:serviceProviders:projects";
        public const string ServiceProviderOffers = "users:serviceProviders:offers";
        public const string ServiceProviderComments = "users:serviceProviders:comments";
        public const string ServiceProviderSkills = "users:serviceProviders:skills";
        public const string ServiceProviderSpecialties = "users:serviceProviders:specialties";
        public const string ServiceProviderProjectFriendly = "users:serviceProviders:projectfriendly";

        public const string UserIdentity = "users:identity";
        public const string Customers = "users:customers";
        public const string CustomerProfile = "users:customers:profile";
        public const string CustomerProjects = "users:customers:projects";

        public const string Editors = "editors";
        public const string Exist = "exist";
        public const string Photographers = "photographers";
        public const string PhoneNumber = "phonenumber";
        public const string Administrators = "administrators";
        public const string Operators = "operators";

        public const string Items = "items";

        public const string Albums = "albums";
        public const string AlbumItems = "albums:items";
        public const string AlbumSettings = "albums:settings";
        public const string Messages = "messages";
        public const string Notifications = "notifications";
        public const string Tickets = "tickets";
        public const string Files = "files";
        public const string FileName = "files:name";
        public const string DiskFile = "files:file";
        public const string FileResponse = "files:response";
        public const string FileInfo = "files:info";
        public const string FileInquiryInfo = "files:info:inquiry";
        public const string Photos = "photos";
        public const string Portfolios = "users:serviceProviders:portfolios";
        public const string OrderItems = "orders:items";

        public const string ProjectSettings = "app:projects:settings";

        public const string Id = "id";
        public const string Ids = "ids";
        public const string Unit = "unit";
        public const string Size = "size";
        public const string ServerInfo = "app:serverInfo";
        public const string AppVersions = "app:versions";
        public const string JudgementItem = "project:judgement:item";


    }
}
