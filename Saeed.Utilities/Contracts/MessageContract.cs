using System.Net;
using System.Runtime.Serialization;

using MessagePack;

using Saeed.Utilities.API.Responses;

namespace Saeed.Utilities.Contracts
{
#pragma warning disable CS1723 // XML comment has cref attribute that refers to a type parameter
    [Serializable]
    [MessagePackObject(true)]
    [DataContract]
    public record MessageContract : MessageContractBase, IMessageContract
    {
        public MessageContract() : base()
        {

        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public MessageContract(string message, List<string> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message, errors, statusCode)
        {
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public MessageContract(string message, HttpStatusCode statusCode) : base(message, statusCode)
        {
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public MessageContract(string message) : base(message)
        {
        }
        public MessageContract(string message, bool success = false) : base(message, success)
        {
        }
    }


    /// <summary>
    /// Generic Message Contract with <see cref="Data"/> or without data. same as <see cref="MessageContract"/>
    /// </summary>
    [Serializable]
    [MessagePackObject(true)]
    [DataContract]
    public record MessageContract<T> : MessageContractBase, IMessageContract<T>
    {
        public MessageContract() : base()
        {
            Success = true;
            StatusCode = HttpStatusCode.OK;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public MessageContract(string message, List<string> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message, errors, statusCode)
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public MessageContract(string message, HttpStatusCode statusCode) : base(message, statusCode)
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public MessageContract(string message) : base(message)
        {

        }

        /// <summary>
        /// auto convert data to message contract type
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator MessageContract<T>(T data)
        {
            return new MessageContract<T>(data: data);
        }

        /// <summary>
        /// response as success result with <see cref="T"/> data by default and 200 (OK) status code.
        /// </summary>
        /// <param name="data"></param>
        public MessageContract(T data) : base()
        {
            Data = data;
            StatusCode = HttpStatusCode.OK;
            Success = true;
        }
        public MessageContract(T data, PagedResponse pagedInfo) : base()
        {
            Data = data;
            Paging = pagedInfo;
            StatusCode = HttpStatusCode.OK;
            Success = true;
        }
        /// <summary>
        /// response as success result with <see cref="T"/> data by default and 200 (OK) status code. include a message
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <param name="success"></param>
        public MessageContract(T data, string message, bool success = true) : base(message)
        {
            Data = data;
            StatusCode = HttpStatusCode.OK;
            Success = success;
        }
        /// <summary>
        /// the data which is must back to receiver.
        /// </summary>
        [Key(1)]
        public T Data { get; set; }
        [Key(2)]
        public virtual PagedResponse Paging { get; set; }
    }
}