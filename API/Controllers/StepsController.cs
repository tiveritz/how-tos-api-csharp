using Microsoft.AspNetCore.Mvc;


namespace HowTosApi.Controllers
{
    [ApiVersion("1")]
    [Route("hwts/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class StepsController : ControllerBase
    {
        private AppDb Db;

        public StepsController(AppDb db)
        {
            this.Db = db;
        }

        [HttpGet]//hwts/v1/steps
        public IActionResult GetAllSteps()
        {
            StepQuery htq = new StepQuery(Db);
            return Ok(htq.GetAll());
        }

        [Route("{id}")] //hwts/v1/steps/d874djd9
        [HttpGet]
        public IActionResult GetStepById(string id)
        {
            StepQuery htq = new StepQuery(Db);
            return Ok(htq.GetOne(id));
        }
    }
}
