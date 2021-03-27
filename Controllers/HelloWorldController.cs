using Microsoft.AspNetCore.Mvc;


namespace how_tos_api.Controllers
{
    [ApiController] // attribute that enables API behaviours
    [Route("[controller]")]
    public class HelloWorldController : ControllerBase
    {

        [HttpGet]
        public string Get()
        {
            HelloWorld hw = new HelloWorld("Hello World");
            return hw.helloWorld;
        }
    }
}
