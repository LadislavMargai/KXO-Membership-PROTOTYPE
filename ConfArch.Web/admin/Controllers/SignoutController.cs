using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ConfArch.Web.admin.Controllers
{
    [Route("api/admin/[controller]")] // musel jsem dat napred "api/", protoze jsem mel problem s excludovani "/admin/API" z reactiho routeru
    [ApiController]
    [AllowAnonymous]
    public class SignoutController : ControllerBase
    {
        // POST api/admin/<ValuesController>
        [HttpPost]
        public IActionResult Post()
        {
            Response.Cookies.Delete("authToken");

            // Jak bude klient vedet, ze jiz neni autentizovany?
            return Ok();
        }
    }
}
