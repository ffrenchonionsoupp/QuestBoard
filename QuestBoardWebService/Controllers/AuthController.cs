using Microsoft.AspNetCore.Mvc;

namespace QuestBoardWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // GET api/auth/verify
        [HttpGet("verify")]
        [BasicAuthentication]
        public IActionResult Verify()
        {
            // Reaching this point means the BasicAuthentication filter
            // already confirmed a well-formed Basic Auth header was sent.
            return Ok(new { message = "Credentials verified via Basic Authentication." });
        }
    }
}
