using System.Runtime.Serialization;
using System;

namespace Saeed.Utilities.Contracts.Domain
{
    /// <summary>
    /// encrypt able entity marker, tables who marked with this interface will be encrypt and decrypt by system.
    /// </summary>
    public interface IEncryptableBaseEntity
    {

    }
    [DataContract]
    [Serializable]
    [MessagePack.MessagePackObject(keyAsPropertyName: true)]
    public class EncryptableBaseEntity : IEncryptableBaseEntity
    {

    }
}