using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kentico.Admin
{
    public class IdentityManager : IIdentityManager
    {
        private static string defaultScheme;
        private static bool authenticateLiveSite;
        public static Dictionary<string, string> AdminExternalAuthentication = new Dictionary<string, string>();


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


        public void SignInEveryWhere(HttpContext httpContext, IUser user)
        {
            if (!string.IsNullOrEmpty(defaultScheme) || authenticateLiveSite)
            {
                httpContext.SignInAsync(defaultScheme, GetPrincipal(user, defaultScheme), new AuthenticationProperties { IsPersistent = false });
            }

            httpContext.SignInAsync(KenticoConstants.AUTHENTICATION_SCHEME, GetPrincipal(user, KenticoConstants.AUTHENTICATION_SCHEME), new AuthenticationProperties { IsPersistent = false });
        }


        public void SignOutEveryWhere(HttpContext httpContext)
        {
            if (!string.IsNullOrEmpty(defaultScheme))
            {
                httpContext.SignOutAsync(defaultScheme);
            }

            httpContext.SignOutAsync(KenticoConstants.AUTHENTICATION_SCHEME);
        }


        public static void RegisterScheme(string scheme, bool authenticateLiveSiteOption)
        {
            defaultScheme = scheme;
            authenticateLiveSite = authenticateLiveSiteOption;
        }


        public static async Task AdministrationAutoSignIn<TOptions>(PrincipalContext<TOptions> principalContext) where TOptions : AuthenticationSchemeOptions
        {
            var userRepository = new UserRepository();
            var user = userRepository.GetByUserName(principalContext.Principal.Identity.Name);

            if (user?.Role.Equals("admin") == true)
            {
                await principalContext.HttpContext.SignInAsync(KenticoConstants.AUTHENTICATION_SCHEME,
                    new IdentityManager().GetPrincipal(user, KenticoConstants.AUTHENTICATION_SCHEME),
                    new AuthenticationProperties { IsPersistent = false });
            }
        }


        public static async Task AdministrationAutoSignOut<TOptions>(PropertiesContext<TOptions> propertiesContext) where TOptions : AuthenticationSchemeOptions
        {
            await propertiesContext.HttpContext.SignOutAsync(KenticoConstants.AUTHENTICATION_SCHEME);
        }
    }
}
