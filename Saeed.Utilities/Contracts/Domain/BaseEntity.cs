using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

using MessagePack;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Saeed.Utilities.Contracts.Domain
{
    /// <summary>
    /// Base Domain / Entities Marker with default int Key. sql and nosql usable
    /// </summary>
    public interface IBaseEntity : IBaseEntity<int>
    {
        //[BsonId]
        //public int Id { get; set; }

        //public DateTimeOffset CreatedOn { get; set; }
        //public DateTimeOffset? LastModified { get; set; }
    }

    /// <summary>
    /// Base Generic Domain / Entities Marker and generic key type (String,int,etc). sql and nosql usable
    /// </summary>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public interface IBaseEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the Id of the Entity.
        /// </summary>
        /// <value>Id of the Entity.</value>
        [BsonId]
        [MaxLength(36)]
        public TKey Id { get; set; }

        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc, Representation = MongoDB.Bson.BsonType.Timestamp)]
        public DateTimeOffset CreatedOn { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc, Representation = MongoDB.Bson.BsonType.Timestamp)]
        public DateTimeOffset? LastModified { get; set; }
    }


    /// <summary>
    /// base entity for int Id (overridable). sql and nosql usable for all business domains.
    /// </summary>
    [DataContract]
    [MessagePackObject(keyAsPropertyName: true)]
    [System.Serializable]
    [BsonIgnoreExtraElements(Inherited = true)]
    //[BsonKnownTypes(typeof(AuditableBaseEntity), typeof(AuditableBaseEntity<>))]
    public abstract class BaseEntity : IBaseEntity<int>
    {
        /// <summary>
        /// Gets or sets the id for this object (the primary record for an entity).
        /// </summary>
        /// <value>The id for this object (the primary record for an entity).</value>
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        [MaxLength(36)]
        public virtual int Id { get; set; }

        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc, Representation = MongoDB.Bson.BsonType.Timestamp)]
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc, Representation = MongoDB.Bson.BsonType.Timestamp)]
        public DateTimeOffset? LastModified { get; set; }

    }

    /// <summary>
    /// generic base entity with T Id for all business domains.
    /// </summary>
    /// <typeparam name="TKey">type of Primary Key / Id Property <see cref="TKey"/></typeparam>
    [DataContract]
    [MessagePackObject(keyAsPropertyName: true)]
    [System.Serializable]
    [BsonIgnoreExtraElements(Inherited = true)]
    //[BsonKnownTypes(typeof(AuditableBaseEntity), typeof(AuditableBaseEntity<>))]
    public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the id for this object (the primary record for an entity).
        /// </summary>
        /// <value>The id for this object (the primary record for an entity).</value>
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        [MaxLength(36)]
        public virtual TKey Id { get; set; }

        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc, Representation = MongoDB.Bson.BsonType.Timestamp)]
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc, Representation = MongoDB.Bson.BsonType.Timestamp)]
        public DateTimeOffset? LastModified { get; set; }
    }

    /// <summary>
    /// event base domain marker
    /// </summary>
    public interface IBaseEventEntity
    {
        public List<BaseDomainEvent> Events { get; set; }

    }

    /// <summary>
    /// for whose domain that need to raise events
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    [System.Serializable]
    [DataContract]
    public abstract class BaseEventEntity : IBaseEventEntity
    {
        public List<BaseDomainEvent> Events { get; set; } = new List<BaseDomainEvent>();

        public bool HasEvent() => Events != null && Events.Any();

    }
}