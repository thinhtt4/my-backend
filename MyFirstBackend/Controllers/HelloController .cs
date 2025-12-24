using Microsoft.AspNetCore.Mvc;

namespace MyFirstBackend.Controllers
{
    [ApiController]
    [Route("api/hello")]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Hello World" });
        }
    }

}
