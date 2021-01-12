using System.Security.Claims;

namespace Kentico.Admin
{
    public interface IIdentityManager
    {
        ClaimsPrincipal GetPrincipal(IUser user, string scheme);
    }
}