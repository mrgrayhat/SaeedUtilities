namespace Saeed.Utilities.DynamicSettings.Site
{
    public class ServerSettings
    {
        public bool UseDomain { get; set; } = false;
        public bool UseHttps { get; set; } = false;
        public string Domain { get; set; }
    }
}
