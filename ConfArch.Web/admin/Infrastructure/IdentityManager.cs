using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kentico.Admin
{
    public class IdentityManager : IIdentityManager
    {
        private static List<string> liveSiteSchemes = new List<string>();


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


        public static Task ConfigureAutomaticSignInForAdministration<TOptions>(PrincipalContext<TOptions> principalContext) where TOptions : AuthenticationSchemeOptions
        {
            var userRepository = new UserRepository();
            var user = userRepository.GetByUserName(principalContext.Principal.Identity.Name);

            if (user?.Role.Equals("admin") == true)
            {
                liveSiteSchemes.Add(principalContext.Scheme.Name);
                principalContext.HttpContext.SignInAsync(KenticoConstants.AUTHENTICATION_SCHEME,
                    new IdentityManager().GetPrincipal(user, KenticoConstants.AUTHENTICATION_SCHEME),
                    new AuthenticationProperties { IsPersistent = false });
            }

            return Task.FromResult(true);
        }


        public static Task ConfigureAutomaticSignOutForAdministration<TOptions>(PropertiesContext<TOptions> propertiesContext) where TOptions : AuthenticationSchemeOptions
        {
            propertiesContext.HttpContext.SignOutAsync(KenticoConstants.AUTHENTICATION_SCHEME);

            return Task.FromResult(true);
        }


        public void SignOutEveryWhere(HttpContext httpContext)
        {
            liveSiteSchemes.ForEach(scheme => httpContext.SignOutAsync(scheme));

            httpContext.SignOutAsync(KenticoConstants.AUTHENTICATION_SCHEME);
        }
    }
}
