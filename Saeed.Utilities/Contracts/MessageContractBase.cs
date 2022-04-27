using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;

using Saeed.Utilities.LocalizedResource.ErrorMessages;
using Saeed.Utilities.LocalizedResource.InformationMessages;
using Saeed.Utilities.API;
using Saeed.Utilities.API.Responses;

using MessagePack;

using Microsoft.AspNetCore.Mvc;

namespace Saeed.Utilities.Contracts
{
    /// <summary>
    /// message contract used for response, message and data transfer between multiple services / apis or internal services.
    /// this is base class of normal <see cref="MessageContract"/> and <see cref="MessageContract{T}"/> top level models.
    /// </summary>
    [Serializable]
    [MessagePackObject(true)]
    [DataContract]
    public abstract record MessageContractBase : IMessageContractBase
    {
        /// <summary>
        /// return simple success result.
        /// </summary>
        public MessageContractBase()
        {
            Success = true;
            StatusCode = HttpStatusCode.OK;
        }

        /// <summary>
        /// return as unsuccessful result by default. with specific message and errors and status code (400 BadRequest by default).
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errors"></param>
        /// <param name="statusCode"></param>
        protected MessageContractBase(string message, List<string> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            Message = message;
            Errors = errors;
            StatusCode = statusCode;
            Success = false;
        }
        /// <summary>
        /// response as unsuccessful result by default. with a message and status code
        /// </summary>
        /// <param name="message">message string</param>
        /// <param name="statusCode">http status code</param>
        protected MessageContractBase(string message, HttpStatusCode statusCode)
        {
            Message = message;
            StatusCode = statusCode;
            Success = false;
        }
        /// <summary>
        /// response as success / unsuccessful (default) result with provided message string.
        /// </summary>
        /// <param name="message">message string</param>
        /// <param name="success">success status : true / false.</param>
        protected MessageContractBase(string message, bool success = false)
        {
            Message = message;
            Success = success;
            StatusCode = success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        }

        /// <summary>
        /// A message that describes an event / situation.
        /// </summary>
        [Key(3)]
        public string Message { get; set; }
        /// <summary>
        /// List of errors and problems
        /// </summary>
        [Key(4)]
        public List<string> Errors { get; set; } = new List<string>();
        /// <summary>
        /// Status ID code, Http standard
        /// </summary>
        [Key(5)]
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// Determines whether the operation is successful or not
        /// </summary>
        [Key(0)]
        public bool Success { get; set; } = true;

        /// <summary>
        /// return formatted string contain status code and message.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"[{StatusCode}]: {Message} .";
        /// <summary>
        /// return true if success == false and error list not empty. useful for ui
        /// </summary>
        /// <returns></returns>
        public bool HasError() => Success == false && Errors.Any();

        /// <summary>
        /// return a not found result with default ErrorMessages.NotFound localized message and HttpStatusCode.NotFound status code (404)
        /// </summary>
        /// <param name="message">custom message</param>
        /// <param name="statusCode"></param>
        /// <returns>A Message Contract object</returns>
        public static MessageContract NotFound(string message = null, HttpStatusCode statusCode = HttpStatusCode.NotFound)
        {
            return new MessageContract(message: message ?? ErrorMessages.NotFound, statusCode);
        }
        /// <summary>
        /// return a not found result with default ErrorMessages.NotFound localized message and HttpStatusCode.NotFound status code (404)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">custom message</param>
        /// <param name="statusCode"></param>
        /// <returns>A Message Contract object</returns>
        public static MessageContract<T> NotFound<T>(string message = null, HttpStatusCode statusCode = HttpStatusCode.NotFound)
        {
            return new MessageContract<T>(message: message ?? ErrorMessages.NotFound, statusCode);
        }

        public static MessageContract Failed()
        {
            return new MessageContract(message: ErrorMessages.ErrorOccurred, statusCode: HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// return a failed / error result with default ErrorMessages.ErrorOccurred localized message and HttpStatusCode.InternalServerError code (500)
        /// </summary>
        /// <param name="message">custom message</param>
        /// <param name="statusCode"></param>
        /// <returns>A Message Contract object</returns>
        public static MessageContract Failed(string message = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            return new MessageContract(message: message ?? ErrorMessages.ErrorOccurred, statusCode: statusCode);
        }
        /// <summary>
        /// return a failed / error result with default ErrorMessages.ErrorOccurred localized message, array of error messages (optional) and HttpStatusCode.InternalServerError code (500)
        /// </summary>
        /// <param name="message">custom message</param>
        /// <param name="errors"></param>
        /// <param name="statusCode"></param>
        /// <returns>A Message Contract object</returns>
        public static MessageContract Failed(string message = null, List<string> errors = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            return new MessageContract(message: message ?? ErrorMessages.ErrorOccurred, errors, statusCode: statusCode);
        }

        /// <summary>
        /// return a failed / error result with default ErrorMessages.ErrorOccurred localized message and HttpStatusCode.InternalServerError code (500)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">custom message</param>
        /// <param name="statusCode"></param>
        /// <returns>A Message Contract object</returns>
        public static MessageContract<T> Failed<T>(string message = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            return new MessageContract<T>(message: message ?? ErrorMessages.ErrorOccurred, statusCode: statusCode);
        }
        /// <summary>
        /// return a failed / error result with default ErrorMessages.ErrorOccurred localized message, array of error messages and HttpStatusCode.InternalServerError code (500)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">custom message</param>
        /// <param name="errors"></param>
        /// <param name="statusCode"></param>
        /// <returns>A Message Contract object</returns>
        public static MessageContract<T> Failed<T>(string message, List<string> errors, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            return new MessageContract<T>(message: message ?? ErrorMessages.ErrorOccurred, errors, statusCode: statusCode);
        }

        /// <summary>
        /// return a bad request result with default ErrorMessages.ValidationError localized message and  HttpStatusCode.BadRequest status code (400)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">custom message</param>
        /// <param name="statusCode"></param>
        /// <returns>A Message Contract object</returns>
        public static MessageContract<T> Bad<T>(string message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new MessageContract<T>(message: message ?? ErrorMessages.ValidationError, statusCode: statusCode);
        }
        public static MessageContract Bad(string message = null, List<string> errors = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new MessageContract(message: message ?? ErrorMessages.ValidationError, errors, statusCode: statusCode);
        }
        public static MessageContract Bad(string message = null)
        {
            return new MessageContract(message: message ?? ErrorMessages.ValidationError, statusCode: HttpStatusCode.BadRequest);
        }
        public static MessageContract Bad()
        {
            return new MessageContract(message: ErrorMessages.ValidationError, statusCode: HttpStatusCode.BadRequest);
        }
        /// <summary>
        /// return a bad request result with default ErrorMessages.ValidationError localized message, an array of error messages and  HttpStatusCode.BadRequest status code (400)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">custom message</param>
        /// <param name="errors"></param>
        /// <param name="statusCode"></param>
        /// <returns>A Message Contract object</returns>
        public static MessageContract<T> Bad<T>(string message, List<string> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new MessageContract<T>(message: message ?? ErrorMessages.ValidationError, errors, statusCode: statusCode);
        }

        public static MessageContract Ok(string message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new MessageContract(message: message ?? InformationMessages.SuccessfulOperation, statusCode: statusCode)
            {
                Success = true
            };
        }
        public static MessageContract Unauthorized(string message = null, HttpStatusCode statusCode = HttpStatusCode.Unauthorized)
        {
            return new MessageContract(message: message ?? ErrorMessages.UnauthorizedResourceAccess, statusCode: statusCode);
        }
        /// <summary>
        /// create an unauthorized access message result with http status code 401 and UnauthorizedResourceAccess error message by default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static MessageContract<T> Unauthorized<T>(string message = null, HttpStatusCode statusCode = HttpStatusCode.Unauthorized)
        {
            return new MessageContract<T>(message: message ?? ErrorMessages.UnauthorizedResourceAccess, statusCode: statusCode);
        }
        /// <summary>
        /// create an unauthorized access message result with http status code 401 and UnauthorizedResourceAccess error message by default, may contain errors.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="errors"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static MessageContract<T> Unauthorized<T>(string message, List<string> errors, HttpStatusCode statusCode = HttpStatusCode.Unauthorized)
        {
            return new MessageContract<T>(message: message ?? ErrorMessages.UnauthorizedResourceAccess, errors, statusCode: statusCode);
        }

        /// <summary>
        /// return an success / ok result with T Data and HttpStatusCode.Ok status code (200)
        /// </summary>
        /// <typeparam name="T">type of data</typeparam>
        /// <param name="data">data obj</param>
        /// <returns>an success message contract </returns>
        public static MessageContract<T> Ok<T>(T data)
        {
            return new MessageContract<T>(data: data);
        }
        public static MessageContract<T> Ok<T>(T data, string message = null)
        {
            return new MessageContract<T>(data: data, message: message);
        }

        /// <summary>
        /// return an paged success / ok result with T Data, paging info like total count, and HttpStatusCode.Ok status code (200)
        /// </summary>
        /// <typeparam name="T">type of data</typeparam>
        /// <param name="data">data obj</param>
        /// <param name="pageNumber">current page no</param>
        /// <param name="pageSize">max page size no</param>
        /// <param name="total">total available records </param>
        /// <returns>an success message contract </returns>
        public static MessageContract<T> Ok<T>(T data, int pageNumber, int pageSize, long total)
        {
            return new MessageContract<T>(data: data,
                pagedInfo: new PagedResponse(pageNumber: pageNumber,
                pageSize: pageSize,
                totalRecords: total));
        }
        /// <summary>
        /// return an paged success / ok result with T Data, paging info like total count, and HttpStatusCode.Ok status code (200)
        /// </summary>
        /// <typeparam name="T">type of data</typeparam>
        /// <param name="data">data obj</param>
        /// <param name="pagedInfo">paging info like total,page and page size</param>
        /// <returns>an success message contract </returns>
        public static MessageContract<T> Ok<T>(T data, PagedResponse pagedInfo)
        {
            return new MessageContract<T>(data: data, pagedInfo: pagedInfo);
        }
    }

    public static class MessageContractExtensions
    {
        /// <summary>
        /// copy old message contract information with new data and data type. like an entity to a dto but same as previous status, paging, message and errors.
        /// </summary>
        /// <typeparam name="TSource">source message type</typeparam>
        /// <typeparam name="TDestination">output/destination message type</typeparam>
        /// <param name="messageContract">old message object</param>
        /// <param name="data">new data</param>
        /// <returns>New Message Contract with same values as old but new data and type.</returns>
        public static MessageContract<TDestination> CopyAs<TSource, TDestination>(this MessageContract<TSource> messageContract, TDestination data)
        {
            return new MessageContract<TDestination>(data: data,
                messageContract.Message, messageContract.Success)
            {
                Paging = messageContract.Paging,
                StatusCode = messageContract.StatusCode,
                Errors = messageContract.Errors
            };
        }

        /// <summary>
        /// act same as CopyAs but clone new objects independent as old objects in memory. 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="messageContract"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static MessageContract<TDestination> CloneAs<TSource, TDestination>(this MessageContract<TSource> messageContract, TDestination data)
        {
            return new MessageContract<TDestination>(data: data,
                messageContract.Message, messageContract.Success)
            {
                Paging = new PagedResponse(messageContract.Paging.PageNumber, messageContract.Paging.PageSize, messageContract.Paging.TotalRecords),
                StatusCode = messageContract.StatusCode,
                Errors = messageContract.Errors.ToList()
            };
        }

        /// <summary>
        /// return a mvc ObjectResult With ApiResponse Model for api's. this handle ResponseType like notfound or unauthorized, bad request, ok and etc with api response data, based on message status code value.
        /// </summary>
        /// <param name="messageContract"></param>
        /// <returns></returns>
        public static ActionResult<ApiResponse> AsApiResponse(this MessageContract messageContract)
        {
            var responseModel = new ApiResponse(messageContract);
            return responseModel.StatusCodeToMvcObjectResult(messageContract.StatusCode);

        }
        /// <summary>
        /// return a mvc ObjectResult With ApiResponse Model for api's. this handle ResponseType like notfound or unauthorized, bad request, ok and etc with api response data, based on message status code value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageContract"></param>
        /// <returns></returns>
        public static ActionResult<ApiResponse<T>> AsApiResponse<T>(this MessageContract<T> messageContract)
        {
            var responseModel = new ApiResponse<T>(data: messageContract.Data,
               messageContract.Message, messageContract.Success)
            {
                //Paging = messageContract.Paging,
                StatusCode = messageContract.StatusCode,
                Errors = messageContract.Errors
            };
            return responseModel.StatusCodeToMvcObjectResult(messageContract.StatusCode);
        }

        public static ActionResult<PagedApiResponse<T>> AsPagedApiResponse<T>(this MessageContract<T> messageContract, PagedResponse paging = null)
        {
            var responseModel = new ApiResponse<T>(data: messageContract.Data,
               messageContract.Message, messageContract.Success)
            {
                Paging = paging ?? messageContract.Paging,
                StatusCode = messageContract.StatusCode,
                Errors = messageContract.Errors
            };
            return responseModel.StatusCodeToMvcObjectResult(messageContract.StatusCode);
        }
    }
}