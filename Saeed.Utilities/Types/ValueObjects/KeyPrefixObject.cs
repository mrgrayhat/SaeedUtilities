
using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Serialization;

using Saeed.Utilities.API.Requests.Parameters;

namespace Saeed.Utilities.Types.ValueObjects
{
    [MessagePack.MessagePackObject(keyAsPropertyName: true)]
    [Serializable]
    [DataContract]
    public class KeyPrefixObject : IEquatable<string>
    {
        public KeyPrefixObject(string prefix)
        {
            Prefix = prefix;
            Argumants = new ArrayList();
        }
        public KeyPrefixObject(string prefix, string[] args)
        {
            Prefix = prefix;
            Argumants = args == null ? new ArrayList() : new ArrayList(args);
        }

        public string Prefix { get; private set; }
        public ArrayList Argumants { get; set; }
        public string Key => string.Concat(Prefix, Argumants.Count > 0 ? ":" + string.Join(":", Argumants.ToArray()) : string.Empty);

        /// <summary>
        /// build the key by concatation of prefix and argumants
        /// </summary>
        /// <returns></returns>
        public string Build()
        {
            return Key;
        }


        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // TODO: write your implementation of Equals() here
            return base.Equals(obj);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return base.GetHashCode();
        }

        public bool Equals(string key)
        {
            return string.Compare(Key, key, CultureInfo.InvariantCulture, CompareOptions.Ordinal) > 1;
            //return Key.Equals(key);
        }

        /// <summary>
        /// build the key string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Key;
        }
        /// <summary>
        /// append new arg to this prefix and update key
        /// </summary>
        /// <param name="arg"></param>
        internal void Append(string arg)
        {
            Argumants.Add(arg);
        }

    }
    public static class KeyPrefixExtenstions
    {
        /// <summary>
        /// append an array of strings as argumants
        /// </summary>
        /// <param name="prefixChain"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static KeyPrefixObject WithArgs(this KeyPrefixObject prefixChain, string[] args)
        {
            prefixChain.Append(string.Concat(string.Join(":", args)));
            return prefixChain;
        }
        public static KeyPrefixObject WithArgs(this KeyPrefixObject prefixChain, string arg)
        {
            prefixChain.Append(arg);
            return prefixChain;
        }
        public static KeyPrefixObject WithId<T>(this KeyPrefixObject prefixChain, T arg)
        {
            prefixChain.Append($"Id:{arg.ToString()}");
            return prefixChain;
        }
        /// <summary>
        /// append paging params to this prefix, can be chained
        /// </summary>
        /// <param name="prefixChain"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static KeyPrefixObject WithPaging(this KeyPrefixObject prefixChain, int page, int size)
        {
            prefixChain.Append(string.Concat($"paging:page:{page}:size:{size}"));
            return prefixChain;
        }
        /// <summary>
        /// append filter params to this prefix, can be chained
        /// </summary>
        /// <param name="prefixChain"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static KeyPrefixObject WithFilters(this KeyPrefixObject prefixChain, string[] arg = null)
        {
            prefixChain.Append(string.Concat($"filters", arg == null ? string.Empty : ":" + string.Join(":", arg)));
            return prefixChain;
        }

        /// <summary>
        /// add request paging and filter argumants And Extra Argumants if needed, to the existing cache key.
        /// </summary>
        /// <param name="prefixChain">key object to update</param>
        /// <param name="filters">paging and sort/search params object (as refrence parameter)</param>
        /// <param name="extraArgs">any other args as string (like arg1:value1:arg2:value2.. )</param>
        /// <returns></returns>
        public static KeyPrefixObject WithFilters(this KeyPrefixObject prefixChain, ref RequestParameterWithSearch filters, string[] extraArgs = null)
        {
            // append paging args
            prefixChain.WithPaging(page: filters.PageNumber, size: filters.PageSize);
            // append filter args
            prefixChain.WithFilters(new[]
            {
                $"sort:{filters.SortOrder}",
                string.IsNullOrEmpty(filters.SearchTerm) ? string.Empty : $"search:{filters.SearchTerm}"
            });
            // append custom args if provided by caller
            if (extraArgs != null && extraArgs.Length > 0)
            {
                prefixChain.WithArgs(extraArgs);
            }

            return prefixChain;
        }
    }
}
