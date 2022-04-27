using System;

namespace Saeed.Utilities.Types.ValueObjects
{
    /// <summary>
    /// full version info object
    /// </summary>
    [Serializable]
    public class VersionValueObject : Object
    {
        public int Major { get; set; } = 0;
        public int Minor { get; set; } = 0;
        public int Build { get; set; } = 0;
        public int Revision { get; set; } = 0;

        /// <summary>
        /// last update time in utc
        /// </summary>
        public DateTime LastModified { get; set; }
        //public bool IsPreRelease { get; set; } = false;
        public bool PreRelease { get; set; } = false;

        /// <summary>
        /// full version string
        /// </summary>
        public string Version => ToString();
        public string FullVersion => ToFullVersion();

        /// <summary>
        /// stringify version numbers
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Major}.{Minor}.{Build}";
        }
        public string ToFullVersion()
        {
            if (PreRelease)
                return $"{Major}.{Minor} Build {Build} Rev{Revision} preRelease";
                    else
                return $"{Major}.{Minor} Build {Build} Rev{Revision}";

        }
    }
}
