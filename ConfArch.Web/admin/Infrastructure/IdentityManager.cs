using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace Kentico.Admin
{
    public class IdentityManager : IIdentityManager
    {
        public ClaimsPrincipal GetPrincipal(IUser user, string scheme)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("FavoriteColor", user.FavoriteColor)
            };

            var identity = new ClaimsIdentity(claims, scheme);

            return new ClaimsPrincipal(identity);
        }
    }
}
