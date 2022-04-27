using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using Saeed.Utilities.LocalizedResource.ErrorMessages;

using MessagePack;

namespace Saeed.Utilities.Contracts.Domain
{
    public interface IAuditableBaseEntity
    {
        /// <summary>
        /// creator identifier
        /// </summary>
        [MaxLength(36, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string CreatedBy { get; set; }

        /// <summary>
        /// editor identifier
        /// </summary>
        [MaxLength(36, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string LastModifiedBy { get; set; }
    }
    /// <summary>
    /// overridable entity base with int Id
    /// </summary>
    [DataContract]
    [MessagePackObject(keyAsPropertyName: true)]
    [System.Serializable]
    public abstract class AuditableBaseEntity : BaseEntity, IAuditableBaseEntity
    {
        [MaxLength(36, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string CreatedBy { get; set; }

        [MaxLength(36, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string LastModifiedBy { get; set; }
    }

    /// <summary>
    /// every class that inherited from Auditable Entity, can audit changes.
    /// </summary>
    /// <typeparam name="TKey">type of Primary Key / Id Property <see cref="TKey"/></typeparam>
    [DataContract]
    [MessagePackObject(keyAsPropertyName: true)]
    [System.Serializable]
    public abstract class AuditableBaseEntity<TKey> : BaseEntity<TKey>, IAuditableBaseEntity
    {
        [MaxLength(36, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public virtual string CreatedBy { get; set; }

        [MaxLength(36, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public virtual string LastModifiedBy { get; set; }
    }

}