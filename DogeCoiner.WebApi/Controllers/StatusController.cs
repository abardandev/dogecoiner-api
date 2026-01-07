using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogeCoiner.Data.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class StatusController : ControllerBase
    {
        public StatusController()
        {
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("server online");
        }
    }
}
