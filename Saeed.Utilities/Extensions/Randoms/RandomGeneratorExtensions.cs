using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Saeed.Utilities.Extensions.Randoms
{
    public static class RandomGeneratorExtensions
    {
        private const string CharPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
        // List of characters and numbers to be used...  
        private static readonly IReadOnlyList<int> NumberCharacters = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
        private static readonly IReadOnlyList<char> AlphabetCharacters = new List<char>()
        {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
            'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B',
            'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
            'Q', 'R', 'S',  'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};

        private static readonly Random Random = new Random(Environment.TickCount);

        /// <summary>
        /// generate a 12 digit (by default) very unique random number
        /// </summary>
        /// <param name="size"></param>
        public static long GenerateRandomDigits(int size = 12)
        {
            return long.Parse(BitConverter.DoubleToInt64Bits(Random.NextDouble()).ToString().Substring(0, size));
        }

        /// <summary>
        /// generate random guid with custom length
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string GenerateUniqueString(int size = 12)
        {
            return Guid.NewGuid().ToString("N").Substring(0, size);
        }

        /// <summary>
        /// generate a 5 digits number by default.
        /// </summary>
        /// <returns></returns>
        public static string GenerateConfirmCode()
        {
            return Random.Next(10000, 99999).ToString("D5");
        }

        /// <summary>
        /// generate a random highly unique base 64 string
        /// </summary>
        /// <returns></returns>
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// generate an highly 10 digits unique code.
        ///  useful for payment ref id / transaction number / personal code and etc.
        /// (ex output: 8538854816)
        /// </summary>
        /// <returns></returns>
        public static string CreateRandomSaleReferenceId()
        {
            return DateTime.Now.AddDays(1).ToString("ff")
                 + DateTime.Now.ToString("ssfff")
                 + new Random().Next(1, 999).ToString("D3");
        }

        /// <summary>
        /// generate a unique guid plus a datetime to increase uniqueness (useful for fileId/filename/token)
        /// </summary>
        /// <returns></returns>
        public static string GenerateDateIdFileName()
        {
            return DateTime.Now.ToString("ssfff") +
                Guid.NewGuid() +
                DateTime.Now.AddDays(1).ToString("ff");
        }

        public static string GenerateShortLink(this string baseUrl, int length = 8)
        {
            //TODO: linked base
            return string.Concat(baseUrl, "/", GenerateRandomString(length));
        }

        /// <summary>
        /// generate a random string contains alphabet and digit chars. 
        /// useful for short  unique links / string / codes.
        /// </summary>
        /// <param name="length">max output length</param>
        /// <returns></returns>
        public static string GenerateRandomString(int length = 8)
        {
            StringBuilder sb = new StringBuilder();

            while (length-- > 0)
                sb.Append(CharPool[(int)(Random.NextDouble() * CharPool.Length)]);

            return sb.ToString();
        }

        /// <summary>
        /// generate a random string contains alphabet and digit chars. 
        /// useful for short  unique links / string / codes.
        /// </summary>
        /// <param name="length">max output length</param>
        /// <returns></returns>
        public static string GenerateRandomString2(int length = 8)
        {
            StringBuilder urlBuilder = new StringBuilder(length);
            // run the loop till I get a string of length characters  

            for (int i = 0; i < length; i++)
            {
                // Get random numbers, to get either a character or a number...  
                int random = Random.Next(0, 3);
                if (random == 1)
                {
                    // use a number  
                    random = Random.Next(0, NumberCharacters.Count);
                    urlBuilder.Append(NumberCharacters[random]);
                }
                else
                {
                    random = Random.Next(0, AlphabetCharacters.Count);
                    urlBuilder.Append(AlphabetCharacters[random]);
                }
            };

            return urlBuilder.ToString();
        }


        /// <summary>
        /// highly performanced random string generator, using parallel for. 
        /// contains alphabet and digit chars. useful for short  unique links / string / codes.
        /// </summary>
        /// <param name="length">max output length</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<string> GenerateRandomString2Async(int length = 8, CancellationToken cancellationToken = default)
        {
            StringBuilder urlBuilder = new StringBuilder(length);

            // run the loop till I get a string of length characters  
            Parallel.For(0, length, new ParallelOptions()
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = Environment.ProcessorCount
            }, i =>
            {
                // Get random numbers, to get either a character or a number...  
                int random = Random.Next(0, 3);
                if (random == 1)
                {
                    // use a number  
                    random = Random.Next(0, NumberCharacters.Count);
                    urlBuilder.Append(NumberCharacters[random]);
                }
                else
                {
                    random = Random.Next(0, AlphabetCharacters.Count);
                    urlBuilder.Append(AlphabetCharacters[random]);
                }
            });
            return Task.FromResult(urlBuilder.ToString());
        }

        /// <summary>
        /// Generate an sequential Guid value that based on datetime ticks.
        /// </summary>
        /// <returns></returns>
        public static Guid GenerateSequentialGuid()
        {
            long _timeCounter = DateTime.UtcNow.Ticks;

            byte[] array = Guid.NewGuid().ToByteArray();
            byte[] bytes = BitConverter.GetBytes(Interlocked.Increment(ref _timeCounter));
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            array[8] = bytes[1];
            array[9] = bytes[0];
            array[10] = bytes[7];
            array[11] = bytes[6];
            array[12] = bytes[5];
            array[13] = bytes[4];
            array[14] = bytes[3];
            array[15] = bytes[2];
            return new Guid(array);
        }
    }
}
