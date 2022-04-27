
using Microsoft.AspNetCore.Http;

namespace Saeed.Utilities.Extensions.Http
{
    public static class HttpContextExtensions
    {
        public static string GetServerHost(this HttpContext context)
        {
            return context.Request.Scheme + "://" + context.Request.Host.ToUriComponent();
        }

        public static string GetServerHost(this HttpContextAccessor context)
        {
            return context.HttpContext.Request.Scheme + "://" + context.HttpContext.Request.Host.ToUriComponent();
        }
        public static string GetServerHost(this HttpRequest request)
        {
            return request.Scheme + "://" + request.Host.ToUriComponent();
        }
    }
}
