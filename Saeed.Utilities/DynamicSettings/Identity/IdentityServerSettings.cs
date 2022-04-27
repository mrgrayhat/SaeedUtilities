namespace Saeed.Utilities.DynamicSettings.Identity
{
    public class IdentityServerSettings
    {
        /// <summary>
        /// auth server url
        /// </summary>
        public string Authority { get; set; }
        /// <summary>
        /// protocol
        /// </summary>
        public string Schema { get; set; } = "https";

    }
}
