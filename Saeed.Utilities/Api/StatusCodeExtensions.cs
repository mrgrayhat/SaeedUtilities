using System.Net;

using Microsoft.AspNetCore.Mvc;

namespace Saeed.Utilities.API
{
    public static class StatusCodeExtensions
    {
        /// <summary>
        /// determine Mvc Object Result type from http status code and return related Result Object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseObject">object / value which is add to result</param>
        /// <param name="statusCode">the status code of result.</param>
        /// <returns></returns>
        public static ActionResult StatusCodeToMvcObjectResult<T>(this T responseObject, HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.Continue => new OkObjectResult(responseObject),
                HttpStatusCode.SwitchingProtocols => new OkObjectResult(responseObject),
                HttpStatusCode.Processing => new OkObjectResult(responseObject),
                HttpStatusCode.EarlyHints => new OkObjectResult(responseObject),
                HttpStatusCode.OK => new OkObjectResult(responseObject),
                HttpStatusCode.Created => new OkObjectResult(responseObject),
                HttpStatusCode.Accepted => new AcceptedResult(),
                HttpStatusCode.NonAuthoritativeInformation => new OkObjectResult(responseObject),
                HttpStatusCode.NoContent => new NoContentResult(),
                HttpStatusCode.ResetContent => new OkObjectResult(responseObject),
                HttpStatusCode.PartialContent => new PartialViewResult(),
                HttpStatusCode.MultiStatus => new OkObjectResult(responseObject),
                HttpStatusCode.AlreadyReported => new StatusCodeResult((int)HttpStatusCode.AlreadyReported),
                HttpStatusCode.IMUsed => new StatusCodeResult((int)HttpStatusCode.IMUsed),
                HttpStatusCode.Ambiguous => new StatusCodeResult((int)HttpStatusCode.Ambiguous),
                //case HttpStatusCode.MultipleChoices:
                //break;
                HttpStatusCode.Moved => new StatusCodeResult((int)HttpStatusCode.Moved),
                //case HttpStatusCode.MovedPermanently:
                //break;
                HttpStatusCode.Found => new StatusCodeResult((int)HttpStatusCode.Found),
                //case HttpStatusCode.Redirect:
                //break;
                HttpStatusCode.RedirectMethod => new StatusCodeResult((int)HttpStatusCode.RedirectMethod),
                //case HttpStatusCode.SeeOther:
                //break;
                HttpStatusCode.UseProxy => new StatusCodeResult((int)HttpStatusCode.UseProxy),
                HttpStatusCode.Unused => new StatusCodeResult((int)HttpStatusCode.Unused),
                HttpStatusCode.RedirectKeepVerb => new StatusCodeResult((int)HttpStatusCode.RedirectKeepVerb),
                //case HttpStatusCode.TemporaryRedirect:
                //break;
                HttpStatusCode.PermanentRedirect => new StatusCodeResult((int)HttpStatusCode.PermanentRedirect),
                HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(responseObject),
                HttpStatusCode.PaymentRequired => new BadRequestObjectResult(responseObject),
                HttpStatusCode.Forbidden => new UnauthorizedObjectResult(responseObject),
                HttpStatusCode.NotFound => new NotFoundObjectResult(responseObject),
                HttpStatusCode.MethodNotAllowed => new StatusCodeResult((int)HttpStatusCode.MethodNotAllowed),
                HttpStatusCode.NotAcceptable => new StatusCodeResult((int)HttpStatusCode.NotAcceptable),
                HttpStatusCode.ProxyAuthenticationRequired => new StatusCodeResult((int)HttpStatusCode.ProxyAuthenticationRequired),
                HttpStatusCode.RequestTimeout => new StatusCodeResult((int)HttpStatusCode.RequestTimeout),
                HttpStatusCode.Conflict => new StatusCodeResult((int)HttpStatusCode.Conflict),
                HttpStatusCode.Gone => new StatusCodeResult((int)HttpStatusCode.Gone),
                HttpStatusCode.LengthRequired => new StatusCodeResult((int)HttpStatusCode.LengthRequired),
                HttpStatusCode.PreconditionFailed => new StatusCodeResult((int)HttpStatusCode.PreconditionFailed),
                HttpStatusCode.RequestEntityTooLarge => new StatusCodeResult((int)HttpStatusCode.RequestEntityTooLarge),
                HttpStatusCode.RequestUriTooLong => new BadRequestObjectResult(responseObject),
                HttpStatusCode.UnsupportedMediaType => new BadRequestObjectResult(responseObject),
                HttpStatusCode.RequestedRangeNotSatisfiable => new StatusCodeResult((int)HttpStatusCode.RequestedRangeNotSatisfiable),
                HttpStatusCode.ExpectationFailed => new StatusCodeResult((int)HttpStatusCode.ExpectationFailed),
                HttpStatusCode.MisdirectedRequest => new StatusCodeResult((int)HttpStatusCode.MisdirectedRequest),
                HttpStatusCode.UnprocessableEntity => new UnprocessableEntityObjectResult(responseObject),
                HttpStatusCode.Locked => new StatusCodeResult((int)HttpStatusCode.Locked),
                HttpStatusCode.FailedDependency => new StatusCodeResult((int)HttpStatusCode.FailedDependency),
                HttpStatusCode.UpgradeRequired => new StatusCodeResult((int)HttpStatusCode.UpgradeRequired),
                HttpStatusCode.PreconditionRequired => new StatusCodeResult((int)HttpStatusCode.PreconditionRequired),
                HttpStatusCode.TooManyRequests => new StatusCodeResult((int)HttpStatusCode.TooManyRequests),
                HttpStatusCode.RequestHeaderFieldsTooLarge => new StatusCodeResult((int)HttpStatusCode.RequestHeaderFieldsTooLarge),
                HttpStatusCode.UnavailableForLegalReasons => new StatusCodeResult((int)HttpStatusCode.UnavailableForLegalReasons),
                HttpStatusCode.InternalServerError => new StatusCodeResult((int)HttpStatusCode.InternalServerError),
                HttpStatusCode.NotImplemented => new StatusCodeResult((int)HttpStatusCode.NotImplemented),
                HttpStatusCode.BadGateway => new BadRequestObjectResult(responseObject),
                HttpStatusCode.ServiceUnavailable => new StatusCodeResult((int)HttpStatusCode.ServiceUnavailable),
                HttpStatusCode.GatewayTimeout => new BadRequestObjectResult(responseObject),
                HttpStatusCode.HttpVersionNotSupported => new StatusCodeResult((int)HttpStatusCode.HttpVersionNotSupported),
                HttpStatusCode.VariantAlsoNegotiates => new StatusCodeResult((int)HttpStatusCode.VariantAlsoNegotiates),
                HttpStatusCode.InsufficientStorage => new StatusCodeResult((int)HttpStatusCode.InsufficientStorage),
                HttpStatusCode.LoopDetected => new StatusCodeResult((int)HttpStatusCode.LoopDetected),
                HttpStatusCode.NotExtended => new StatusCodeResult((int)HttpStatusCode.NotExtended),
                HttpStatusCode.NetworkAuthenticationRequired => new StatusCodeResult((int)HttpStatusCode.NetworkAuthenticationRequired),
                HttpStatusCode.NotModified => new StatusCodeResult((int)HttpStatusCode.NotModified),
                HttpStatusCode.BadRequest => new BadRequestObjectResult(responseObject),
                _ => new BadRequestObjectResult(responseObject),
            };
        }
    }
}
