namespace Saeed.Utilities.Extensions.Caching
{
    public static class CacheKeyPrefixBuilder
    {
        public static string BuildKeyPrefix(this string prefix, string[] args)
        {
            return string.Concat(prefix, ":", string.Join(":", args));
        }
    }
}
