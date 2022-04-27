using System;

using Saeed.Utilities.Extensions.Strings;

namespace Saeed.Utilities.DynamicSettings.Firebase
{
    public class FirebaseSettings
    {
        private string _jsonServiceAccountFcmkey;

        public string JsonServiceAccountFcmkey
        {
            get
            {
                return _jsonServiceAccountFcmkey;
            }
            set
            {
                _jsonServiceAccountFcmkey = value.DecodeBase64String();
            }
        }
        public string AuthUriGoogleCredentialScope { get; set; }
        public string FcmNewRestApiUri { get; set; }
        public string PriorityFcmMessages { get; set; }
        public int NotificationTimeToLive { get; set; } = 86400;
    }
}