using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using MediatR;

namespace Saeed.Utilities.Contracts.Domain
{
    public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : BaseDomainEvent
    {
        public DomainEventNotification(TDomainEvent domainEvent)
        {
            DomainEvent = domainEvent;
        }

        public TDomainEvent DomainEvent { get; }
    }


    public interface IBaseDomainEvent : INotification
    {
        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc, Representation = MongoDB.Bson.BsonType.Timestamp)]
        public DateTime DateOccurred { get; set; }
    }

    /// <summary>
    /// base class for domains which have events
    /// </summary>
    [DataContract]
    [MessagePack.MessagePackObject(keyAsPropertyName: true)]
    [System.Serializable]
    public class BaseDomainEvent : IBaseDomainEvent, INotification
    {
        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc, Representation = MongoDB.Bson.BsonType.Timestamp)]
        public DateTime DateOccurred { get; set; } = DateTime.UtcNow;
    }
}