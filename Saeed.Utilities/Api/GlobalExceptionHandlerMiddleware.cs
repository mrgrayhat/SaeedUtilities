using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Saeed.Utilities.LocalizedResource.ErrorMessages;
using Saeed.Utilities.API.Responses;
using Saeed.Utilities.Exceptions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Saeed.Utilities.API
{
    /// <summary>
    /// this middleware handle exceptions and error globally, and return formatted json response. useful for api's / services, without need to handle every exceptions manually. 
    /// just use app.UseMiddleware<![CDATA[<]]>GlobalExceptionHandlerMiddleware<![CDATA[>]]>() to enable.
    /// </summary>
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private static bool _isDevelopment;

        #region serilization and response settings

        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            //Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            IgnoreNullValues = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        private const string ResponsesContentType = "application/json; charset=utf-8";

        #endregion

        /// <summary>
        /// initial config
        /// </summary>
        static GlobalExceptionHandlerMiddleware()
        {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            // check if environment name is available or not. if it's available compare and set is development. otherwise set to false.
            _isDevelopment = !string.IsNullOrEmpty(env) && env == Environments.Development || env == Environments.Staging;
            if (_isDevelopment)
                Console.WriteLine($"{nameof(GlobalExceptionHandlerMiddleware)} Is Configured For Development Environment.");
            else
                Console.WriteLine($"{nameof(GlobalExceptionHandlerMiddleware)} Is Configured For Production Environment.");
        }

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Invoke(HttpContext context/*, IWebHostEnvironment environment*/)
        {
            try
            {
                await _next.Invoke(context);
                // global 404 handler
                //if (!context.Response.HasStarted)
                //{
                //    if (context.Response.StatusCode == 404)
                //    {
                //context.Response.ContentType = ContentType;
                //        var response = new ApiResponseBase
                //        {
                //            Message = ErrorMessages.PageOrResourceNotFound
                //        };
                //        var json = System.Text.Json.JsonSerializer.Serialize(response, _serializerOptions);

                //await context.Response.WriteAsync(json, Encoding.UTF8);
                //    }
                //}
            }
            catch (NotFoundException notfoundException)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = ResponsesContentType;
                    context.Response.StatusCode = 404;

                    var response = new ApiResponseBase()
                    {
                        Message = notfoundException.Message ?? ErrorMessages.NotFound,
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                    };
                    if (_isDevelopment)
                    {
                        _logger.LogError(notfoundException, $"a unhandled {nameof(NotFoundException)} raised !");
                        response.Errors = new List<string> {
                            notfoundException.Message.ToString(),
                            notfoundException.InnerException?.ToString(),
                            notfoundException.StackTrace
                        };
                    }
                    var json = System.Text.Json.JsonSerializer.Serialize(response, _serializerOptions);

                    await context.Response.WriteAsync(json, Encoding.UTF8);
                }
            }
            catch (ArgumentException argumantException)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = ResponsesContentType;
                    context.Response.StatusCode = 400;

                    var response = new ApiResponseBase
                    {
                        Message = argumantException.Message ?? ErrorMessages.ValidationError,
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        TraceIdentifier = context.TraceIdentifier,
                        ApiVersion = context.GetRequestedApiVersion()?.ToString() ?? "1.0"
                    };
                    if (_isDevelopment)
                    {
                        _logger.LogError(argumantException, $"a unhandled {nameof(ArgumentException)} raised !");
                        response.Errors = new List<string> {
                            argumantException.Message.ToString(),
                            argumantException.InnerException?.ToString(),
                            argumantException.StackTrace
                        };
                    }
                    var json = System.Text.Json.JsonSerializer.Serialize(response, _serializerOptions);

                    await context.Response.WriteAsync(json, Encoding.UTF8);
                }
            }
            catch (DeleteFailureException deleteFailedException)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = ResponsesContentType;
                    context.Response.StatusCode = 400;

                    var response = new ApiResponseBase
                    {
                        Message = deleteFailedException.Message ?? ErrorMessages.FailedOperation,
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                    };
                    if (_isDevelopment)
                    {
                        _logger.LogError(deleteFailedException, $"a unhandled {nameof(DeleteFailureException)} raised !");
                        response.Errors = new List<string> {
                            deleteFailedException.Message.ToString(),
                            deleteFailedException.InnerException?.ToString(),
                            deleteFailedException.StackTrace
                        };
                    }
                    var json = System.Text.Json.JsonSerializer.Serialize(response, _serializerOptions);

                    await context.Response.WriteAsync(json, Encoding.UTF8);
                }
            }
            catch (ApiException apiException)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = ResponsesContentType;
                    context.Response.StatusCode = 400;

                    var response = new ApiResponseBase
                    {
                        Message = apiException.Message ?? ErrorMessages.ErrorOccurred,
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                    };
                    if (_isDevelopment)
                    {
                        _logger.LogError(apiException, $"a unhandled {nameof(ApiException)} raised !");
                        response.Errors = new List<string> {
                            apiException.Message.ToString(),
                            apiException.InnerException?.ToString(),
                            apiException.StackTrace
                        };
                    }
                    var json = System.Text.Json.JsonSerializer.Serialize(response, _serializerOptions);

                    await context.Response.WriteAsync(json, Encoding.UTF8);
                }
            }
            catch (BadRequestException badRequestException)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = ResponsesContentType;
                    context.Response.StatusCode = 400;

                    var response = new ApiResponseBase
                    {
                        Message = badRequestException.Message ?? ErrorMessages.ValidationError,
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                    };
                    if (_isDevelopment)
                    {
                        _logger.LogError(badRequestException, $"a unhandled {nameof(BadRequestException)} raised !");
                    }
                    var json = System.Text.Json.JsonSerializer.Serialize(response, _serializerOptions);

                    await context.Response.WriteAsync(json, Encoding.UTF8);
                }
            }
            catch (ValidationException validationException)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = ResponsesContentType;
                    context.Response.StatusCode = 400;

                    var response = new ApiResponseBase
                    {
                        Message = validationException.Message ?? ErrorMessages.ValidationError,
                        Errors = validationException.Errors,
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                    };
                    if (_isDevelopment)
                    {
                        _logger.LogError(validationException, $"a unhandled {nameof(ValidationException)} raised !");
                        response.Errors = new List<string> {
                            validationException.Message.ToString(),
                            validationException.InnerException?.ToString(),
                            validationException.StackTrace
                        };
                    }
                    var json = System.Text.Json.JsonSerializer.Serialize(response, _serializerOptions);

                    await context.Response.WriteAsync(json, Encoding.UTF8);
                }
            }
            catch (AuthenticationException authenticationException)
            {
                _logger.LogError(authenticationException, $"an {nameof(AuthenticationException)} raised !");
                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = ResponsesContentType;
                    context.Response.StatusCode = 403;
                    var response = new ApiResponseBase
                    {
                        TraceIdentifier = context.TraceIdentifier,
                        Message = ErrorMessages.ResourceAccessLimited,
                        StatusCode = System.Net.HttpStatusCode.Unauthorized,
                    };
                    if (_isDevelopment)
                    {
                        response.Errors = new List<string> {
                            authenticationException.Message.ToString(),
                            authenticationException.InnerException?.ToString(),
                            authenticationException.StackTrace
                        };
                    }
                    var json = System.Text.Json.JsonSerializer.Serialize(response, _serializerOptions);

                    await context.Response.WriteAsync(json, Encoding.UTF8);
                }
            }
            catch (Exception exception)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = ResponsesContentType;
                    context.Response.StatusCode = 500;
                    var response = new ApiResponseBase
                    {
                        TraceIdentifier = context.TraceIdentifier,
                        Message = ErrorMessages.ErrorOccurred,
                        StatusCode = System.Net.HttpStatusCode.InternalServerError,
                        Success = false,
                    };
                    if (_isDevelopment)
                    {
                        _logger.LogError(exception, "a unhandled Exception has been raised !");
                        response.Errors = new List<string> {
                            exception.Message.ToString(),
                            exception.InnerException?.ToString(),
                            exception.StackTrace
                        };
                    }

                    var json = System.Text.Json.JsonSerializer.Serialize(response, _serializerOptions);

                    await context.Response.WriteAsync(json, Encoding.UTF8);
                }
            }
        }
    }
}
