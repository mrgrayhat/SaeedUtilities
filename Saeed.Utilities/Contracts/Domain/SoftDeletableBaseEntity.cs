using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using Huddeh.LocalizedResource.ErrorMessages;

using MongoDB.Bson.Serialization.Attributes;

namespace Saeed.Utilities.Contracts.Domain
{
    /// <summary>
    /// soft delete able entity marker
    /// </summary>
    public interface ISoftDeletableBaseEntity
    {
        public bool IsDeleted { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc, Representation = MongoDB.Bson.BsonType.Timestamp)]
        DateTimeOffset? DeletedOn { get; set; }
        //[MaxLength(36, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string DeletedBy { get; set; }
    }

    /// <summary>
    /// <inheritdoc cref="ISoftDeletableBaseEntity"/>
    /// </summary>
    [DataContract]
    [Serializable]
    [MessagePack.MessagePackObject(keyAsPropertyName: true)]
    public class SoftDeletableBaseEntity : ISoftDeletableBaseEntity
    {
        public bool IsDeleted { get; set; } = false;
        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc, Representation = MongoDB.Bson.BsonType.Timestamp)]
        public DateTimeOffset? DeletedOn { get; set; }
        [MaxLength(36, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string DeletedBy { get; set; }
    }
}