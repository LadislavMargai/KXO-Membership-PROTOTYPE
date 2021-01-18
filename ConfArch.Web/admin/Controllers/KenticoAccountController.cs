using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult Login(LoginModel credentials)
        {
            if (credentials?.UserName == null)
            {
                return Unauthorized();
            }

            var user = userRepository.GetByUsernameAndPassword(credentials.UserName, credentials.Password);
            if (user == null || user.Role != "admin")
            {
                return Unauthorized();
            }

            identityManager.SignInEveryWhere(HttpContext, user);

            return Ok();
        }


        [HttpPost]
        public IActionResult Logout()
        {
            identityManager.SignOutEveryWhere(HttpContext);

            return Ok();
        }


        [Authorize]
        public IActionResult MyProfile()
        {
            var currentUser = userRepository.GetByUserName(HttpContext.User.Identity.Name);
            if (currentUser == null || currentUser.Role != "admin")
            {
                return BadRequest();
            }
            
            return Ok(new UserInfo(currentUser));
        }


        public IActionResult ExternalAuthenticationMethods()
        {
            return Ok(IdentityManager.AdminExternalAuthentication.Select(pair => new { Url = pair.Key, Name = pair.Value }));
        }
    }
}
