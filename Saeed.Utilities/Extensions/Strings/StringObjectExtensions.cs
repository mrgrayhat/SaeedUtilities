using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using MongoDB.Bson;

namespace Saeed.Utilities.Extensions.Strings
{
    public static class StringObjectExtensions
    {
        #region Mongo
        /// <summary>
        /// make a unique bson identifier (for mongo db Id /Key's) and return as string.
        /// </summary>
        /// <param name="datetime">to feed randomizer</param>
        /// <returns></returns>
        public static string GenerateBsonId(DateTime datetime)
        {
            return ObjectId.GenerateNewId(datetime).ToString();
        }
        /// <summary>
        /// make a unique bson identifier (for mongo db Id /Key's) and return as string.
        /// </summary>
        /// <returns></returns>
        public static string GenerateBsonId()
        {
            return ObjectId.GenerateNewId().ToString();
        }

        public static ObjectId GenerateBsonObject()
        {
            return ObjectId.GenerateNewId();
        }

        #endregion

        public static bool IsBase64String(this string base64String)
        {
            try
            {
                _ = Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string EncodeBase64(this string value)
        {
            try
            {
                var buffer = Encoding.UTF8.GetBytes(value);
                return Convert.ToBase64String(buffer);
            }
            catch
            {
                Console.Out.WriteLineAsync($"base64 string decode failed! {Activity.Current?.TraceId.ToString()}");
                return null;
            }
        }

        public static string DecodeBase64String(this string base64String)
        {
            try
            {
                byte[] decodedBytes = Convert.FromBase64String(base64String);
                return Encoding.UTF8.GetString(decodedBytes);
            }
            catch
            {
                Console.Out.WriteLineAsync($"base64 string decode failed! {Activity.Current?.TraceId.ToString()}");
                return null;
            }
        }
        public static string DecodeBase64Bytes(this byte[] base64Bytes)
        {
            try
            {
                return Encoding.UTF8.GetString(base64Bytes);
            }
            catch
            {
                Console.Out.WriteLineAsync($"base64 bytes decode failed! {Activity.Current?.TraceId.ToString()}");
                return null;
            }
        }
        /// <summary>
        /// To demonstrate extraction of file extension from base64 string.
        /// </summary>
        /// <param name="base64String">base64 string.</param>
        /// <returns>Henceforth file extension from string.</returns>
        public static string GetBase64FileExtension(string base64String)
        {
            try
            {
                var data = base64String.Substring(0, 5);

                return data.ToUpper() switch
                {
                    "IVBOR" => "png",
                    "/9J/4" => "jpg",
                    "AAAAF" => "mp4",
                    "JVBER" => "pdf",
                    "AAABA" => "ico",
                    "UMFYI" => "rar",
                    "E1XYD" => "rtf",
                    "U1PKC" => "txt",
                    "MQOWM" => "srt",
                    "77U/M" => "srt",
                    _ => string.Empty,
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static async Task<string> EncodeTextAsync(string text)
        {
            using var memory = new MemoryStream();
            using var writer = new StreamWriter(memory);
            HtmlEncoder.Default.Encode(writer, text);
            using var streamReader = new StreamReader(memory);
            return await streamReader.ReadToEndAsync();
        }

        /// <summary>
        /// remove char / words lile <![CDATA[&amp]]>, <![CDATA[&;]]>, ; and etc from encoded url.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveUnsafeEncodedChars(this string text)
        {
            return text
                .Replace("&amp;", "&") // in winNT
                .Replace("&amp", "&") // in winNT
                .Replace("&;", "&") // in unix / linux
                .Replace("amp", "")
                .Replace("amp;", "");
        }

        /// <summary>
        /// use Html Encoder and encode provided string to valid http url.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string EncodeSafeUrl(this string text)
        {
            return HtmlEncoder.Default.Encode(text).RemoveUnsafeEncodedChars();
        }

        public static string SecureDirtyText(string text)
        {
            return StringSanitizer.UseWhere(text);
        }

    }
}
