
using System.Threading.Tasks;

using Hangfire.Annotations;
using Hangfire.Dashboard;

using Saeed.Utilities.Constants;
using Saeed.Utilities.Extensions.Auth;

namespace Saeed.Utilities.Web.Annotations
{
    /// <summary>
    /// limit hangfire dashboard access to authorized role / policies
    /// </summary>
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public virtual bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            //Allow authenticated admin and operator users to see the Dashboard.
            return httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated &&
                   (httpContext.User.IsInIdentityRole(IdentitySystemConstants.AdministratorRoleName) ||
                    httpContext.User.IsInIdentityRole(IdentitySystemConstants.OperatorRoleName));
        }
    }
    /// <summary>
    /// test limit hangfire dashboard access to authorized role / policies
    /// </summary>
    public class FakeHangfireDashboardAuthorizationFilter : HangfireDashboardAuthorizationFilter
    {
        public override bool Authorize(DashboardContext context) => true;
    }

    /// <summary>
    /// limit hangfire dashboard access to authorized role / policies
    /// </summary>
    public class AsyncHangfireDashboardAuthorizationFilter : IDashboardAsyncAuthorizationFilter
    {
        public virtual Task<bool> AuthorizeAsync(DashboardContext context)
        {
            var user = context.GetHttpContext().User;
            //Allow authenticated admin and operator users to see the Dashboard.
            return Task.FromResult(user.Identity != null && user.Identity.IsAuthenticated &&
                   (user.IsInIdentityRole(IdentitySystemConstants.AdministratorRoleName) ||
                    user.IsInIdentityRole(IdentitySystemConstants.OperatorRoleName)));
        }
    }

    /// <summary>
    /// test limit hangfire dashboard access to authorized role / policies
    /// </summary>
    public class FakeAsyncHangfireDashboardAuthorizationFilter : IDashboardAsyncAuthorizationFilter
    {
        public virtual Task<bool> AuthorizeAsync(DashboardContext context)
        {
            return Task.FromResult(true);
        }
    }

}