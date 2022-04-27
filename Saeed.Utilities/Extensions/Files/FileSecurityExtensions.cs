using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Saeed.Utilities.Extensions.Files
{
    public static class FileSecurityExtensions
    {
        /// <summary>
        /// calculate the byte array MD5 Hash
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string CalculateMD5ByteHash(this byte[] bytes)
        {
            using var md5 = MD5.Create();
            return Encoding.ASCII.GetString(md5.ComputeHash(bytes));
        }
        /// <summary>
        /// synchronously open file stream and calculate MD5 file Hash.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string CalculateFileMD5Hash(this string filePath)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filePath);
            var hash = md5.ComputeHash(stream);

            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        /// asynchronously open an temp file stream to calc md5 file Hash
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<string> CalculateFileMD5HashAsync(this string filePath, CancellationToken cancellationToken)
        {
            await using var fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192);

            return await fStream.GetHashAsync<MD5>(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// asynchronously calc md5 hash of input file that match with <paramref name="hash"/> or not.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="hash"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<bool> CalculateIntegrityAsync(this IFormFile file, string hash,
            CancellationToken cancellationToken)
        {
            await using var fStream = file.OpenReadStream();
            var hashed = await fStream.GetHashAsync<MD5>(cancellationToken).ConfigureAwait(false);

            return Equals(hashed, hash);
        }
        public static async Task<string> CalculateSHA256FileHashAsync(this IFormFile file, CancellationToken cancellationToken)
        {
            await using var fStream = file.OpenReadStream();
            return await fStream.GetHashAsync<SHA256>(cancellationToken).ConfigureAwait(false);
        }
        public static async Task<string> CalculateMD5FileHashAsync(this IFormFile file, CancellationToken cancellationToken)
        {
            await using var fStream = file.OpenReadStream();
            return await fStream.GetHashAsync<MD5>(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// asynchronously compute an stream using <typeparamref name="T"/> algorithm type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<string> GetHashAsync<T>(this Stream stream, CancellationToken cancellationToken)
            where T : HashAlgorithm
        {
            StringBuilder sb = new StringBuilder();
            MethodInfo create = typeof(T).GetMethod("Create", Array.Empty<Type>());
            using T crypt = (T)create.Invoke(null, null);

            if (crypt != null)
            {
                byte[] hashBytes = await crypt.ComputeHashAsync(stream, cancellationToken).ConfigureAwait(false);
                foreach (var t in hashBytes)
                    sb.Append(t.ToString("x2"));
            }
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }
            return sb.ToString();
        }
    }
}
