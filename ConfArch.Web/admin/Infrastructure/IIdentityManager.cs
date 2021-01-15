using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Kentico.Admin
{
    public interface IIdentityManager
    {
        ClaimsPrincipal GetPrincipal(IUser user, string scheme);


        void SignInEveryWhere(HttpContext httpContext, IUser user);


        void SignOutEveryWhere(HttpContext httpContext);
    }
}