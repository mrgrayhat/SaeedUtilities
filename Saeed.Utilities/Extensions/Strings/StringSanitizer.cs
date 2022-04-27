using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Saeed.Utilities.Extensions.Strings
{
    /// <summary>
    /// secure string / text's and clean'em up from invalid, dangerious and dirty chars. use it to clean Text Contents or Messages which is sent from client side or 3rd parties to decrease the chance of xss and injections in Web / Database or pages. each method have diffrent performance and memory allocation but same result. they all can remove dangerious and invalid characters like symboles.
    /// </summary>
    public class StringSanitizer
    {
        /// <summary>
        /// secure using regex. Slower Speed
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        //by MSDN http://msdn.microsoft.com/en-us/library/844skk0h(v=vs.71).aspx
        public static string UseRegex(string strIn)
        {
            // Replace invalid characters with empty strings.
            return Regex.Replace(strIn, @"[^\w\.@-]", "");
        }

        /// <summary>
        /// secure using string builder. Very Slower Speed
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        // by Paolo Tedesco
        public static string UseStringBuilder(string strIn)
        {
            const string removeChars = " ?&^$#@!()+-,:;<>’\'-_*";
            // specify capacity of StringBuilder to avoid resizing
            StringBuilder sb = new StringBuilder(strIn.Length);
            foreach (char x in strIn.Where(c => !removeChars.Contains(c)))
            {
                sb.Append(x);
            }
            return sb.ToString();
        }

        /// <summary>
        /// secure using StringBuilder and HashSet. Normal Speed
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        // by Paolo Tedesco, but using a HashSet
        public static string UseStringBuilderWithHashSet(string strIn)
        {
            var hashSet = new HashSet<char>(" ?&^$#@!()+-,:;<>’\'-_*");
            // specify capacity of StringBuilder to avoid resizing
            StringBuilder sb = new StringBuilder(strIn.Length);
            foreach (char x in strIn.Where(c => !hashSet.Contains(c)))
            {
                sb.Append(x);
            }
            return sb.ToString();
        }

        /// <summary>
        /// secure using StringBuilder and HashSet. Faster Speed
        /// </summary>
        /// <param name="dirtyString"></param>
        /// <returns></returns>
        // by SteveDog
        public static string UseStringBuilderWithHashSet2(string dirtyString)
        {
            HashSet<char> removeChars = new HashSet<char>(" ?&^$#@!()+-,:;<>’\'-_*");
            StringBuilder result = new StringBuilder(dirtyString.Length);
            foreach (char c in dirtyString)
                if (removeChars.Contains(c))
                    result.Append(c);
            return result.ToString();
        }

        /// <summary>
        /// secure using StringBuilder and HashSet. Fast Speed
        /// </summary>
        /// <param name="dirtyString"></param>
        /// <returns></returns>
        // original by patel.milanb
        public static string UseReplace(string dirtyString)
        {
            string removeChars = " ?&^$#@!()+-,:;<>’\'-_*";
            string result = dirtyString;

            foreach (char c in removeChars)
            {
                result = result.Replace(c.ToString(), string.Empty);
            }

            return result;
        }
        /// <summary>
        /// secure using StringBuilder and HashSet. Fastest Speed
        /// </summary>
        /// <param name="dirtyString"></param>
        /// <returns></returns>
        // by L.B
        public static string UseWhere(string dirtyString)
        {
            return new string(dirtyString.Where(char.IsLetterOrDigit).ToArray());
        }
    }

}
