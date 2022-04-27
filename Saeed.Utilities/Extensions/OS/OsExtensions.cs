using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Saeed.Utilities.Extensions.OS
{
    public static class OsExtensions
    {

        public static PlatformID Detect()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    return PlatformID.Win32NT;
                case PlatformID.Unix:
                    return PlatformID.Unix;
                default:
                    throw new PlatformNotSupportedException();
            }
        }
        public static Version DetectVersion()
        {
            return Environment.OSVersion.Version;
        }
        public static bool IsWindowsNT()
        {
            return OperatingSystem.IsWindows();
        }
        public static bool IsWindowsNTVersion(int majorReleaseVersion, int minorReleaseVersion = 0, int buildNumber = 0, int revision = 0)
        {
            return OperatingSystem.IsWindowsVersionAtLeast(major: majorReleaseVersion, minor: minorReleaseVersion, build: buildNumber, revision: revision);
        }
        public static bool IsLinuxOrUnix()
        {
            return OperatingSystem.IsLinux();
        }
        public static bool IsBSD()
        {
            return OperatingSystem.IsFreeBSD();
        }
        public static bool IsMac()
        {
            return OperatingSystem.IsMacOS();
        }

        public static string TempFolderPath()
        {
            return Path.GetTempPath();
        }

        public static string SystemPath()
        {
            return Environment.SpecialFolder.Windows.ToString();
        }
        public static bool IsAndroid(this string userAgent)
        {
            return userAgent.StartsWith("okhttp") || Regex.IsMatch(userAgent, "(Android|Windows Phone|iPad|iPod)", RegexOptions.IgnoreCase);
        }
        public static bool IsIos(this string userAgent)
        {
            return userAgent.StartsWith("darwin") || Regex.IsMatch(userAgent, "(BlackBerry|webOS|iPhone|IEMobile)", RegexOptions.IgnoreCase);
        }
        public static bool IsWindows(string userAgent)
        {
            return !Regex.IsMatch(userAgent, "(BlackBerry|webOS|iPhone|IEMobile|Android|Windows Phone|iPad|iPod)", RegexOptions.IgnoreCase);
        }
    }
}
