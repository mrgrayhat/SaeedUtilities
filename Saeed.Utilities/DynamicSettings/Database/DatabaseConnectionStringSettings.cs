using System.ComponentModel.DataAnnotations;

namespace Saeed.Utilities.DynamicSettings.Database
{
    /// <summary>
    /// all databas providers connection strings
    /// </summary>
    public class DatabaseConnectionStringSettings
    {
        public string LocalDb { get; set; } = null;
        public string SqlServer { get; set; } = null;
        public string SqLite { get; set; } = null;
        public string InMemory { get; set; } = "InMemoryDb";
        public string TestSqlServer { get; set; } = null;
        public string Default { get; set; } = null;
        public string MongoDb { get; set; } = null;
        public string Docker { get; set; } = null;

        public string HangfireSql { get; set; }
        public string HangfireRedis { get; set; }
    }
}
