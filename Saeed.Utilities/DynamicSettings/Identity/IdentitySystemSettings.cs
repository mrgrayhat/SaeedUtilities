namespace Saeed.Utilities.DynamicSettings.Identity
{
    public class IdentitySystemSettings
    {
        /// <summary>
        /// any character which is not contain in this string, isn't valid.
        /// </summary>
        public string LegalUsernameChars { get; set; } = "abcdefghigklmnopqrstuvwxyz1234567890@."; // 

        /// <summary>
        /// password complexy level define a set of settings like at least 1 unique char | digits | non alphanumeric char | upper and lower case chars in the password text. default is 2, which is mean password must contain at least 1 upper case char, digit and at least 1 unique char or symbol. 1 is medium level and only require a combination of chars and  digits. 0 is easiest level without any policy but only min length.
        /// </summary>
        public int PasswordComplexyLevel { get; set; } = 2;
        /// <summary>
        /// the minimum length of password that use can set.
        /// </summary>
        public int MinimumPasswordLength { get; set; } = 6;

        /// <summary>
        /// if you are mobile required / based app.
        /// </summary>
        public bool RequireUniquePhoneNumber { get; set; } = true;
        /// <summary>
        /// require phone number confirmation to sign in.
        /// </summary>
        public bool RequireConfirmedPhoneNumber { get; set; } = true;
        /// <summary>
        /// require email address confirmation to sign in. false if email is optional.
        /// </summary>
        public bool RequireConfirmedEmail { get; set; } = false;
        /// <summary>
        /// false, if email is optional.
        /// </summary>
        public bool RequireUniqueEmail { get; set; } = false;
        /// <summary>
        /// require account verification / confirmation to sign in.
        /// </summary>
        public bool RequireConfirmedAccount { get; set; } = false;

        /// <summary>
        /// how much user account must be locked, after a set of failed attempts.
        /// </summary>
        public int AccountLockoutInMinutes { get; set; } = 3;
        /// <summary>
        /// count of failed password / code attempts before lock out occure.
        /// </summary>
        public int AttemptsBeforeLockout { get; set; } = 5;

    }
}
