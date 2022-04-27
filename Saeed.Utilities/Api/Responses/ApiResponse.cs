using System;
using System.Collections.Generic;
using System.Net;

using Saeed.Utilities.Contracts;

using MessagePack;

namespace Saeed.Utilities.API.Responses
{
    [System.Runtime.Serialization.DataContract]
    [MessagePackObject(true)]
    [Serializable]
    public record PagedApiResponse<T> : MessageContract<T>, IApiResponse<T>
    {
        [Key(6)]
        public string ApiVersion { get; set; }
        [Key(7)]
        public string TraceIdentifier { get; set; }

        /// <summary>
        /// empty success result by default, or custom.
        /// </summary>
        public PagedApiResponse() : base()
        {

        }
        public PagedApiResponse(MessageContract messageContract) : base()
        {
            Success = messageContract.Success;
            Errors = messageContract.Errors;
            StatusCode = messageContract.StatusCode;
        }
        public PagedApiResponse(MessageContract<T> messageContract) : base(messageContract)
        {
            Success = messageContract.Success;
            Errors = messageContract.Errors;
            StatusCode = messageContract.StatusCode;
            Data = messageContract.Data;
        }

        public PagedApiResponse(string message, List<string> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message, errors, statusCode)
        {
        }

        public PagedApiResponse(string message, HttpStatusCode statusCode) : base(message, statusCode)
        {
        }

        public PagedApiResponse(string message) : base(message)
        {
        }

        public PagedApiResponse(T data) : base(data)
        {
        }
        public PagedApiResponse(T data, PagedResponse paging) : base(data, paging)
        {

        }

        public PagedApiResponse(T data, string message) : base(data, message)
        {
        }
        public PagedApiResponse(T data, string message, bool success) : base(data, message, success)
        {
        }
    }

    [System.Runtime.Serialization.DataContract]
    [MessagePackObject]
    [Serializable]
    public record ApiResponse<T> : MessageContract<T>, IApiResponse<T>
    {
        [Key(6)]
        public string ApiVersion { get; set; }
        [Key(7)]
        public string TraceIdentifier { get; set; }

        /// <summary>
        /// empty success result by default, or custom.
        /// </summary>
        public ApiResponse() : base()
        {

        }
        public ApiResponse(MessageContract messageContract) //: base(messageContract.Message, messageContract.Errors, messageContract.StatusCode)
        {
            Message = messageContract.Message;
            Success = messageContract.Success;
            Errors = messageContract.Errors;
            StatusCode = messageContract.StatusCode;
        }
        public ApiResponse(MessageContract<T> messageContract) //: base(messageContract.Data, messageContract.Message, messageContract.Success)
        {
            Success = messageContract.Success;
            Errors = messageContract.Errors;
            StatusCode = messageContract.StatusCode;
            Data = messageContract.Data;
        }

        public ApiResponse(string message, List<string> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message, errors, statusCode)
        {
        }

        public ApiResponse(string message, HttpStatusCode statusCode) : base(message, statusCode)
        {
        }

        public ApiResponse(string message) : base(message)
        {
        }

        public ApiResponse(T data) : base(data)
        {
        }

        [System.Obsolete("use paged api response type instead")]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        [System.Runtime.Serialization.IgnoreDataMember]
        [MessagePack.IgnoreMember]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        public override PagedResponse Paging { get; set; } = null;
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member

        public ApiResponse(T data, string message, bool success) : base(data, message, success)
        {
        }
    }

    [System.Runtime.Serialization.DataContract]
    [MessagePackObject(true)]
    [Serializable]
    public record ApiResponse : ApiResponseBase, IApiResponse
    {
        public ApiResponse(MessageContract messageContract) : base(messageContract)
        {
            Success = messageContract.Success;
            Errors = messageContract.Errors;
            StatusCode = messageContract.StatusCode;
        }

        /// <summary>
        /// empty success result by default, or custom.
        /// </summary>
        public ApiResponse() : base()
        {
            Success = true;
        }
        public ApiResponse(string message, List<string> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base()
        {
            Success = false;
            Message = Message;
            Errors = errors;
            StatusCode = statusCode;
        }

        public ApiResponse(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base()
        {
            Success = false;
            Message = Message;
            StatusCode = statusCode;
        }

        public ApiResponse(string message) : base()
        {
            Success = false;
            Message = Message;
            StatusCode = HttpStatusCode.BadRequest;
        }

    }
}