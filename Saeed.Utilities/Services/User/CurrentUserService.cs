using System;

using Microsoft.AspNetCore.Http;

using Saeed.Utilities.Extensions.Auth;

namespace Saeed.Utilities.Services.User
{
    public interface ICurrentUserService
    {
        public string StringUserId { get; set; }
        Guid UserId { get; }
        bool IsAuthenticated { get; }

    }
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            StringUserId = httpContextAccessor.HttpContext?.GetIdentityUserId();
            Guid.TryParse(StringUserId, out var userId);
            UserId = userId;
            IsAuthenticated = UserId != null;
        }

        public Guid UserId { get; }
        public string StringUserId { get; set; }
        public bool IsAuthenticated { get; }
    }

}
