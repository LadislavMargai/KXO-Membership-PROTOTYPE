using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kentico.Admin
{
    // Musel jsem dat napred "api/", protoze jsem mel problem s excludovani "/admin/API" z reactiho routeru
    [Route("api/admin/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class KenticoAccountController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IIdentityManager identityManager;


        public KenticoAccountController(IUserRepository userRepository, IIdentityManager identityManager)
        {
            this.userRepository = userRepository;
            this.identityManager = identityManager;
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginModel credentials)
        {
            if (credentials?.UserName == null)
            {
                return Unauthorized();
            }

            var user = userRepository.GetByUsernameAndPassword(credentials.UserName, credentials.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            await HttpContext.SignInAsync(KenticoConstants.AUTHENTICATION_SCHEME,
                identityManager.GetPrincipal(user, KenticoConstants.AUTHENTICATION_SCHEME),
                new AuthenticationProperties { IsPersistent = false });

            return Ok();
        }


        [HttpPost]
        public IActionResult Logout()
        {
            identityManager.SignOutEveryWhere(HttpContext);

            // Je otazka, jestli redirectovat na serveru ... protoze se jedna o XHR request. A nebo ma byt Logout request full post?
            // Client si to pak nacachuje a uz me to nechce potom podruhe odhlasit. Rovnou ten XHR logou request redirectne na "/" a nic se nestane
            return Redirect("/");
        }


        [Authorize(AuthenticationSchemes = KenticoConstants.AUTHENTICATION_SCHEME)]
        public IActionResult MyProfile()
        {
            // TODO: Dostanu se sem, ikdyz nejsem autentizovany
            var currentUser = userRepository.GetByUserName(HttpContext.User.Identity.Name);
            if (currentUser == null)
            {
                return BadRequest();
            }
            
            return Ok(new UserInfo(currentUser));
        }
    }
}
