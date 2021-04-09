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

        [HttpGet]
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
            Step newStep = newSq.GetStepById(uriId);

            return CreatedAtAction(nameof(GetStepById), new { id = uriId }, newStep);
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult GetStepById(string id)
        {
            StepQuery sq = new StepQuery(Db);
            return Ok(sq.GetStepById(id));
        }
    }
}
