namespace Saeed.Utilities.DynamicSettings.Caching
{
    public class CachingSettings
    {
        public string Serializer { get; set; } = "messagepack";
        public string Compression { get; set; } = "lz4";
        public string RedisCacheServerIp { get; set; } = "127.0.0.1";
        public int RedisCacheServerPort { get; set; } = 6379;
        public int CompressionLevel { get; set; } = 11;

        public bool Enabled { get; set; } = true;
        public bool EnableRedis { get; set; } = true;
        public bool EnableLogging { get; set; } = true;
        public bool EnableCompression { get; set; } = true;
    }
}
