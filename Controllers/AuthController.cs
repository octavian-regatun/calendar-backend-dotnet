using Microsoft.AspNetCore.Mvc;
using static calendar_backend_dotnet.Auth.AuthService;

namespace calendar_backend_dotnet.Controllers
{
    [Route("api/auth/loggedIn")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        [LoggedInFilter]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}