using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Autobarn.Website.Controllers.api {
    [Route("api")]
    [ApiController]
    public class DefaultController : ControllerBase {
        [HttpGet]
        public IActionResult Get() {
            var result = new {
                _links = new {
                    vehicles = new {
                        href = "/api/vehicles"
                    }
                },
                message = "Welcome to the Autobarn API",
                version = Assembly.GetExecutingAssembly().FullName,
            };
            return Ok(result);
        }
    }
}
