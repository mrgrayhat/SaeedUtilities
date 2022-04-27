namespace Saeed.Utilities.DynamicSettings.Database
{
    /// <summary>
    /// a class that store related database settings such as available providers, connection strings, extra configurations like logging and caching options. you can fill and use this class staticly or register in you Dependency Injection Container to resolve it between the services and reload if source (json file/etc) changed.
    /// </summary>
    public class DatabaseSettings
    {
        public bool EnableRetryOnFailure { get; set; } = true;
        public bool EnableLogging { get; set; } = false;

        public bool UseDocker { get; set; } = false;
        public bool UseLocalDb { get; set; } = false;
        public bool UseSqlServer { get; set; } = false;
        public bool UseSqlite { get; set; } = false;
        public bool UseInMemory { get; set; } = false;
        public bool UseMongoDb { get; set; } = false;

        public string SchedulerDbProvider { get; set; } = "HangfireRedis";

        public string DatabaseName { get; set; }
        /// <summary>
        /// Providers Connection String.
        /// </summary>
        public DatabaseConnectionStringSettings ConnectionStrings { get; set; }
    }
}
