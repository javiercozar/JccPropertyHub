using Microsoft.AspNetCore.Mvc;

namespace JccPropertyHub.WebApi.Controllers {
    [ApiController]
    [Route("[Controller]")]
    public class Test : Controller {
        // GET
        [HttpGet]
        public string Index() {
            return "Test OK";
        }
    }
}