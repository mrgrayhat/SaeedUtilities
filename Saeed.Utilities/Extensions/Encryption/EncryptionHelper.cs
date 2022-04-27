using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Saeed.Utilities.Extensions.Encryption
{
    public static class EncryptionHelper
    {
        public const int KeyIterations = 10_000;

        /// <summary>
        /// generate secure encryption key using CryptoService random bytes and RFC289
        /// </summary>
        /// <param name="keyLength"></param>
        /// <param name="salt">extra bytes add to encryption making key stronger. generate randomly if not provided</param>
        /// <returns>an strong encryption key</returns>
        public static string GenerateEncryptionKey(int keyLength = 64, byte[] salt = null)
        {
            if (salt is null || salt.Length <= 0)
                salt = GenerateSalt(keyLength);
            using var rfc289 = new Rfc2898DeriveBytes(Guid.NewGuid().ToString("N"), salt, KeyIterations);
            return Convert.ToBase64String(rfc289.GetBytes(keyLength));
        }

        /// <summary>
        /// generate an striing encryption key using Rfc2898 provider.
        /// </summary>
        /// <param name="password">secret key to use. if null, i use a unique guid</param>
        /// <param name="salt">salt bytes to inject. if null, i use 32  random bytes </param>
        /// <param name="keyIterations">how many time bytes must randomized. default is 10_000 times. Greater Iterations == More secure </param>
        /// <returns></returns>
        public static Rfc2898DeriveBytes GenerateRfcEncryptionKey(string password = null, byte[] salt = null, int keyIterations = KeyIterations)
        {
            if (salt is null || salt.Length <= 0)
                salt = GenerateSalt(32);
            //http://stackoverflow.com/questions/2659214/why-do-i-need-to-use-the-rfc2898derivebytes-class-in-net-instead-of-directly
            //"What it does is repeatedly hash the user password along with the salt." High iteration counts.
            password ??= Guid.NewGuid().ToString("N");
            var key = new Rfc2898DeriveBytes(password, salt, keyIterations);

            return key;
        }

        /// <summary>
        /// generate random salt bytes
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] GenerateSalt(int size = 32)
        {
            //Generate a cryptographic random number.
            //#if NET6_0_OR_GREATER
            //            return RandomNumberGenerator.GetBytes(size);
            //#endif
            using var rng = RandomNumberGenerator.Create();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);

            return buff;
        }

        /// <summary>
        /// this is aes alg (symmetric encryption) helper, that encrypt <paramref name="stream"/> bytes using provided <paramref name="key"/> and <paramref name="salt"/> and other optional parameters that define <paramref name="cipherMode"/>, <paramref name="keySize"/>, <paramref name="paddingMode"/>, <paramref name="maxBuffer"/>.
        /// as result, a new stream must back which is encrypted.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="key">a Rfc2898 Encyption Key. you can generate a secure key by EncryptionHelper.GenerateRfcEncryptionKey(passwordStr, saltBytes) helper.</param>
        /// <param name="salt">salts are random bytes that written to the beginning of the output / encrypted stram, so in this case can be random every time.To make it harder to crack the cryptography key and decrypt data. </param>
        /// <param name="maxBuffer"></param>
        /// <param name="keySize"></param>
        /// <param name="blockSize"></param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<MemoryStream> EncryptSymmetricAsync(this Stream stream, Rfc2898DeriveBytes key, byte[] salt, int maxBuffer = 81_920, int keySize = 256, int blockSize = 128, CipherMode cipherMode = CipherMode.CFB, PaddingMode paddingMode = PaddingMode.PKCS7, CancellationToken cancellationToken = default)
        {
            //Set Rijndael symmetric encryption algorithm
            //using var aes = new RijndaelManaged
            //{
            //    KeySize = 256,
            //    BlockSize = 128,
            //    Padding = PaddingMode.PKCS7
            //};
            using var aes = Aes.Create();
            aes.KeySize = keySize;
            aes.Padding = paddingMode;

            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
            aes.Mode = cipherMode;

            // write salt to the beginning of the output file, so in this case can be random every time.To make it harder to crack the cryptography key and decrypt data.
            var encryptedStream = new MemoryStream();
            await encryptedStream.WriteAsync(salt.AsMemory(0, salt.Length), cancellationToken);

            var cs = new CryptoStream(encryptedStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            //create a buffer so only this amount will allocate in the memory and not the whole file
            byte[] buffer = new byte[maxBuffer];
            //var buffer = ArrayPool<byte>.Shared.Rent(maxBuffer); // array pool is optimizer than new byte[]
            int read;
            try
            {
                while ((read = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken)) > 0)
                {
                    await cs.WriteAsync(buffer.AsMemory(0, read), cancellationToken);
                }
                //NOTE: Important steps:
                // Write final block and clear buffer force push all data to save at destination
                await cs.FlushFinalBlockAsync(cancellationToken);
                // final step: seek to the beginning of the stream, So it can be read again in return
                encryptedStream.Seek(0, SeekOrigin.Begin);
            }
            catch (Exception e)
            {
                Debug.Fail("Encryption failed!", e.Message);
                cs?.Dispose();
                encryptedStream?.Dispose();

                throw;
            }
            finally
            {
                //ArrayPool<byte>.Shared.Return(buffer);
            }

            // return the encrypted stream (actually it's reference in memory)
            return encryptedStream;
        }
    }
}
