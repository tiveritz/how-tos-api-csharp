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

        [HttpGet]//hwts/v1/Steps
        public IActionResult GetAllSteps()
        {
            StepsQuery htq = new StepsQuery(Db);
            return Ok(htq.GetAll());
        }
        
        [HttpPost]
        public IActionResult CreateStep([FromBody]CreateStep createStep)
        {
            StepsQuery sq = new StepsQuery(Db);
            string uriId = sq.CreateStep(createStep);

            StepQuery newSq = new StepQuery(Db);
            Step newStep = newSq.GetOne(uriId);

            return CreatedAtAction(nameof(GetStepById), new { id = uriId }, newStep);
        }

        [Route("{id}")] //hwts/v1/Steps/d874djd9
        [HttpGet]
        public IActionResult GetStepById(string id)
        {
            StepQuery sq = new StepQuery(Db);
            return Ok(sq.GetOne(id));
        }
    }
}
