using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Saeed.Utilities.Extensions.Auth
{
    /// <summary>
    /// Identity User and claims extenstion methods
    /// </summary>
    public static class IdentityExtenstions
    {
        /// <summary>
        /// get a list of messages from identity result type
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static List<string> GetIdentityResultErrors(this IdentityResult result)
        {
            List<string> validationMessages = new List<string>(result.Errors.Count());

            foreach (IdentityError error in result.Errors)
            {
                validationMessages.AddRange(collection: result.Errors.Select(selector: error => $"{error.Code} : {error.Description}"));
            }

            return validationMessages;
        }

        /// <summary>
        /// get errors from IdentityResult Class and return an minified code + description key/value type
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static IDictionary GetIdentityResultErrorsDictionary(this IdentityResult result)
        {
            var validationMessage = new Dictionary<string, string>(result.Errors.Count());
            foreach (IdentityError err in result.Errors)
            {
                validationMessage.TryAdd(err.Code, err.Description);
            }

            return validationMessage;
        }

        /// <summary>
        /// get identity server user id
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetIdentityUserId(this IPrincipal principal)
        {
            return principal.Identity.GetIdentityUserId();
        }
        /// <summary>
        /// get identity server user id from claims
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string GetIdentityUserId(this IIdentity identity)
        {
            return ((identity as ClaimsIdentity).FindFirst("sub") ?? throw new InvalidOperationException("sub claim is missing"))!.Value;
        }
        /// <summary>
        /// get user id from signalr hunbs
        /// </summary>
        /// <param name="hubCallerContext"></param>
        /// <returns></returns>
        public static string GetUserId(this HubCallerContext hubCallerContext)
        {
            return hubCallerContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        /// <summary>
        /// get identity user user identifier for signalr hubs
        /// </summary>
        /// <param name="hubCallerContext"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string GetIdentityUserId(this HubCallerContext hubCallerContext)
        {
            return ((hubCallerContext.User.Identity as ClaimsIdentity).FindFirst("sub") ?? throw new InvalidOperationException("sub claim is missing"))!.Value;
        }

        /// <summary>
        /// get identity server user id from http
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetIdentityUserId(this HttpContext httpContext)
        {
            return (httpContext.User.Identity as ClaimsIdentity).FindFirst("sub")?.Value;// ?? throw new InvalidOperationException("sub claim is missing"))!.Value;
        }

        /// <summary>
        /// Get UserId
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static string GetUserId(this HttpContext httpContext)
        {
            return httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        /// <summary>
        /// get current connection ip which is requested
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetUserIp(this HttpContext httpContext)
        {
            try
            {
                return httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedIp)
                    ? (string)forwardedIp
                    : httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public static bool IsLocalHost(this HttpContext httpContext)
        {
            var local = httpContext?.Request?.Host.Host;

            // request is locally
            if (local is null)
            {
                return true;
            }

            return local.Equals("localhost", StringComparison.OrdinalIgnoreCase) ||
                local.Equals("::1", StringComparison.OrdinalIgnoreCase) ||
                local.Equals("127.0.0.1", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// find user name in claims
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }
        public static string GetIdentityUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst("name")?.Value;
        }

        public static string GetIdentityUserEmail(this ClaimsPrincipal user)
        {
            return user.FindFirst("email")?.Value;
        }
        public static string GetUserEmail(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value;
        }

        public static string GetUserMobilePhone(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.MobilePhone)?.Value;
        }
        public static string GetIdentityUserMobilePhone(this ClaimsPrincipal user)
        {
            return user.FindFirst("phone_number")?.Value;
        }

        public static string GetUserLocality(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Locality)?.Value;
        }
        public static string GetIdentityUserLocality(this ClaimsPrincipal user)
        {
            return user.FindFirst("locale")?.Value;
        }
        /// <summary>
        /// find user timezone in claims
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetTimeZone(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.StateOrProvince)?.Value;
        }
        public static string GetIdentityUserTimeZone(this ClaimsPrincipal user)
        {
            return user.FindFirst("state")?.Value;
        }
        public static TimeSpan GetTimeZoneOffset(this ClaimsPrincipal user)
        {
            var tz = user.FindFirst(ClaimTypes.StateOrProvince)?.Value;
            if (tz is null)
                return TimeSpan.Zero;
            return TimeZoneInfo.FindSystemTimeZoneById(tz).BaseUtcOffset;

        }

        public static string GetUserFirstName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.GivenName).Value;
        }

        public static string GetSecurityStamp(this ClaimsPrincipal user)
        {
            var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;

            return user.FindFirst(securityStampClaimType)?.Value;
        }

        public static List<string> GetUserRoles(this ClaimsPrincipal user)
        {
            var list = new List<string>();

            var roles = user.FindAll(ClaimTypes.Role);
            list.AddRange(roles.Select(item => item.Value));

            return list;
        }

        /// <summary>
        /// get identity server user roles from principal / claims
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static List<string> GetIdentityUserRoles(this ClaimsPrincipal claims)
        {
            return claims.FindAll("role")
                .Select(x => x.Value)
                .ToList();
        }

        /// <summary>
        /// check identity server user role is in principal / claims
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static bool IsInIdentityRole(this ClaimsPrincipal claims, string role)
        {
            return claims.HasClaim("role", role);
        }
        /// <summary>
        /// check identity server user roles is exist in principal / claims
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static bool IsInIdentityRoles(this
            ClaimsPrincipal claims, List<string> roles)
        {
            //return claims.Claims.Any(x => roles.Contains(x.Value));
            return claims.HasClaim(x => roles.Contains(x.Value));
        }
        /// <summary>
        /// get identity user roles for signalr hubs
        /// </summary>
        /// <param name="hubCallerContext"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string GetIdentityUserRoles(this HubCallerContext hubCallerContext)
        {
            return ((hubCallerContext.User.Identity as ClaimsIdentity).FindFirst("role") ?? throw new InvalidOperationException("role claim is missing"))!.Value;
        }

        /// <summary>
        /// get identity server user roles from http
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static List<string> GetIdentityUserRoles(this HttpContext httpContext)
        {
            return (httpContext.User.Identity as ClaimsIdentity).FindAll("role").Select(x => x.Value).ToList();
        }

    }
}
