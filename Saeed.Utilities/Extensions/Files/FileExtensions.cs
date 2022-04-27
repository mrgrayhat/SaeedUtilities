using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace Saeed.Utilities.Extensions.Files
{
    public static class FileExtensions
    {
        private static char[] _invalidFileNameChars;

        public static char[] InvalidFileNameChars
        {
            get
            {
                return _invalidFileNameChars ??= Path.GetInvalidFileNameChars();

            }
        }

        /// <summary>
        /// convert all win nt's path seperators to unix format ( \ to / )
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToUnixPath(this string path)
        {
            return path.Replace('\\', '/');
        }

        /// <summary>
        /// convert all path seperators to win nt format ( / to \ )
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToWindowsPath(this string path)
        {
            return path.Replace('/', '\\');
        }

        /// <summary>
        /// try to extract file MIME Type using <see cref="FileExtensionContentTypeProvider"/> or return application/octet-stream if couldn't find type.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns> file mime or application/octet-stream if not found</returns>
        public static string GetMimeType(this string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        /// <summary>
        /// extract uploading form data file type from headers
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetContentType(this IFormFile file)
        {
            return file.ContentType;
        }

        /// <summary>
        /// extract file extension from <see cref="IFormFile"/> name.
        /// </summary>
        /// <param name="file">the full name/path of the file</param>
        /// <returns>image.png => png</returns>
        public static string GetFileExtension(this IFormFile file)
        {
            return file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower();
        }

        /// <summary>
        /// normalize file name / strin,  and replace invalid characters in name using <see cref=" Path.GetInvalidFileNameChars()"/> with <paramref name="replace"/> or remove them by default.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="replace">replcae with a string or remove unsafe chars by default</param>
        /// <returns></returns>
        public static string SafeName(this string fileName, string replace = "")
        {
            for (var index = 0; index < InvalidFileNameChars.Length; index++)
            {
                var c = InvalidFileNameChars[index];
                fileName = fileName.Replace(c.ToString(), replace);
            }

            return fileName;
        }
        /// <summary>
        /// <inheritdoc cref="SafeName(string, string)"/>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static string SafeName(this IFormFile file, string replace = "")
        {
            return file.Name.SafeName(replace);
        }

        /// <summary>
        /// extract form data file name without it's extension
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(this IFormFile file)
        {
            return Path.GetFileNameWithoutExtension(file.FileName);
        }

        /// <summary>
        /// Returns file size in MB
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static int GetFileSizeMb(this IFormFile file)
        {
            try
            {
                return Convert.ToInt32(file.Length / 1048576.0);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// <inheritdoc cref="Uri.IsWellFormedUriString"/>
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool IsValidUri(this string uri, UriKind kind = UriKind.Absolute)
        {
            return Uri.IsWellFormedUriString(uri, kind);
        }

        /// <summary>
        /// get a directory total files size, and it's subdirectories recursively bu default
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="recursive">enumerate sub directories or only root dir</param>
        /// <returns></returns>
        public static long GetDirectorySize(this DirectoryInfo directoryInfo, bool recursive = true)
        {
            var startDirectorySize = default(long);
            if (directoryInfo == null || !directoryInfo.Exists)
            {
                return startDirectorySize; //Return 0 while Directory does not exist.
            }
            //Add size of files in the Current Directory to main size.
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                Interlocked.Add(ref startDirectorySize, fileInfo.Length);
            }
            //Loop on Sub Direcotries in the Current Directory and Calculate it's files size.
            if (recursive)
            {
                Parallel.ForEach(directoryInfo.GetDirectories(), (subDirectory) =>
                {
                    Interlocked.Add(ref startDirectorySize, subDirectory.GetDirectorySize(recursive));
                });
            }
            //Return full Size of this Directory.
            return startDirectorySize;
        }

        /// <summary>
        /// get a directory total files size, and it's subdirectories recursively bu default
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="recursive">enumerate sub directories or only root dir</param>
        /// <returns></returns>
        public static (long Size, long Count) GetDirectoryFilesCountAndSize(this DirectoryInfo directoryInfo, bool recursive = true)
        {
            var startDirectorySize = default(long);
            var startDirectoryFileCount = default(long);

            if (directoryInfo == null || !directoryInfo.Exists)
            {
                //Return 0 while Directory does not exist.
                return (Size: startDirectorySize, Count: 0);
            }
            //Add size of files in the Current Directory to main size.
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                Interlocked.Add(ref startDirectorySize, fileInfo.Length);
            }
            //Loop on Sub Direcotries in the Current Directory and Calculate it's files size.
            if (recursive)
            {
                Parallel.ForEach(directoryInfo.GetDirectories(), (subDirectory) =>
                {
                    Interlocked.Add(ref startDirectorySize, subDirectory.GetDirectorySize(recursive));
                });
            }

            // count files
            Interlocked.Add(ref startDirectoryFileCount, directoryInfo.CountGetDirectoryFilesCount(recursive));

            //Return full Size of this Directory and total files count.
            return (Size: startDirectorySize, Count: startDirectoryFileCount);
        }

        public static (long Size, long Count) GetDirectoryFilesCountAndSize(this string dirPath, bool recursive = true)
        {
            var startDirectorySize = default(long);
            var startDirectoryFileCount = default(long);

            var directoryInfo = new DirectoryInfo(dirPath);
            if (directoryInfo == null || !directoryInfo.Exists)
            {
                //Return 0 while Directory does not exist.
                return (Size: startDirectorySize, Count: 0);
            }
            //Add size of files in the Current Directory to main size.
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                Interlocked.Add(ref startDirectorySize, fileInfo.Length);
            }
            //Loop on Sub Direcotries in the Current Directory and Calculate it's files size.
            if (recursive)
            {
                Parallel.ForEach(directoryInfo.GetDirectories(), (subDirectory) =>
                {
                    Interlocked.Add(ref startDirectorySize, subDirectory.GetDirectorySize(recursive));
                });
            }

            // count files
            Interlocked.Add(ref startDirectoryFileCount, dirPath.CountGetDirectoryFilesCount(recursive));

            //Return full Size of this Directory and total files count.
            return (Size: startDirectorySize, Count: startDirectoryFileCount);
        }

        public static long CountGetDirectoryFilesCount(this DirectoryInfo directoryInfo, bool recursive = true)
        {
            return directoryInfo.EnumerateFiles("*",
                recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                .LongCount();
        }

        public static long CountGetDirectoryFilesCount(this string dirPath, bool recursive = true)
        {
            return Directory.EnumerateFiles(dirPath,
                "*",
                recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                .LongCount();
        }


    }
}