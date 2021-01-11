using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ConfArch.Web.admin.Controllers
{
    [Route("api/admin/[controller]")] // musel jsem dat napred "api/", protoze jsem mel problem s excludovani "/admin/API" z reactiho routeru
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        // POST api/admin/<ValuesController>
        [HttpPost]
        public IActionResult Post(LoginModel credentials)
        {
            if (credentials == null)
            {
                return Unauthorized();
            }

            IUser user = UserRetriever.GetUser(credentials.UserName, credentials.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            CookieOptions option = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(30),
                HttpOnly = true
            };

            Response.Cookies.Append("authToken", "authTokenValue", option);

            // Jak bude klient vedet, ze je autentizovany?
            return new JsonResult(user);
        }
    }


    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public interface IUser
    {
        Guid Guid { get; set; }
        string Name { get; set; }
        string Email { get; set; }
        bool IsAdmin { get; set; }
    }


    public class User : IUser
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }

    public static class UserRetriever
    {
        public static IUser GetUser(string username, string password)
        {
            IUser user = null;

            if (username == "administrator")
            {
                user = new User
                {
                    Guid = Guid.Empty,
                    Name = username,
                    Email = "admin@localhost",
                    IsAdmin = true
                };
            }

            return user;
        }
    }
}
